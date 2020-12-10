using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CaseNetworking;

public class TeamManagement : MonoBehaviour
{
    public static TeamManagement instance = null;

    public GameObject teamViewPrefab;

    public Transform teamParent;

    [HideInInspector]
    public List<TeamView> teamViews = new List<TeamView>();

    public Button addSlotButton;

    [Header("Operations")]
    public RemoveScoreView removeScoreView;

    public KickTeamView kickTeamView;
    public AddScoreView addScoreView;

    private int joinedTeamNb;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        addSlotButton.onClick.AddListener(() => OnClickAddSlotButton());
    }

    private void Start()
    {
        Server.instance.clientDisconnectionEvent += OnTeamLeft;
    }

    public void OnTeamJoin(int teamNb, int connectionId)
    {
        TeamView view = TeamNumberToTeamView(teamNb);

        if (view != null)
        {
            if (view.connectionId == -1)
            {
                view.isOccupied = true;
                view.connectionId = connectionId;
                view.ActivateTeamView();
                CaseLogs.instance.AddToLog("Client " + connectionId + " have joined " + teamNb);
                joinedTeamNb++;
                AdminServerManagement.instance.OnChangeCurrentTeamNumber(joinedTeamNb);
            }
        }
    }

    public void OnTeamLeft(int connectionId)
    {
        TeamView view = connectionIdToTeamView(connectionId);

        if (view != null && view.isOccupied)
        {
            view.isOccupied = false;
            view.totalScore = 0;
            view.ActivateTeamSlot();
            joinedTeamNb--;
            AdminServerManagement.instance.OnChangeCurrentTeamNumber(joinedTeamNb);
            CaseLogs.instance.AddToLog(string.Format("Team {0} has left the game.", view.teamNb));
        }
    }

    public void OnAdministrationStart(int nbTeams)
    {
        //generate slots
    }

    public void SpawnSlots()
    {
        Debug.Log(GameConfigManager.instance.gameData.nbTeams);
        Debug.Log("Yeehaww");
        for (int i = 1; i <= GameConfigManager.instance.gameData.nbTeams; i++)
        {
            AddTeamSlot(i);
        }

        addSlotButton.transform.SetAsLastSibling();
    }

    public void AddTeamSlot(int slotNb)
    {
        TeamView view = Instantiate(teamViewPrefab, teamParent).GetComponent<TeamView>();
        teamViews.Add(view);
        view.Init(slotNb);
        // CaseLogs.instance.AddToLog("Added a team slot");
    }

    public void OnClickAddSlotButton()
    {
        GameConfigManager.instance.gameData.nbTeams += 1;
        AdminServerManagement.instance.OnChangeTeams(GameConfigManager.instance.gameData.nbTeams);
        AddTeamSlot(FindAvailableSlot());
        addSlotButton.transform.SetAsLastSibling();
        //maybe improve to add a slot at the empty indexes at the beginning
    }

    private int FindAvailableSlot()
    {
        int max = GameConfigManager.instance.gameData.nbTeams;
        for (int i = 1; i < max; i++)
        {
            if (!teamViews.Exists(view => view.teamNb == i))
            {
                return i;
            }
        }
        return max;
    }

    public void DeleteTeamSlot(int nb)
    {
        GameConfigManager.instance.gameData.nbTeams -= 1;
        AdminServerManagement.instance.OnChangeTeams(GameConfigManager.instance.gameData.nbTeams);
        //Update the team NB to not be available for join any longer
        TeamView view = teamViews.Find(x => x.slotNb == nb);
        teamViews.Remove(view);
        CaseLogs.instance.AddToLog(string.Format("Removed team slot number {0}", nb));
        Destroy(view.gameObject);
    }

    public TeamView TeamNumberToTeamView(int teamNb)
    {
        return teamViews.Find(view => view.teamNb == teamNb);
    }

    public void AddTeamSlot()
    {
        GameConfigManager.instance.gameData.nbTeams += 1;
    }

    public int TeamNbToConnectionId(int teamNb)
    {
        int nb = teamViews.Count;
        for (int i = 0; i < nb; i++)
        {
            if (teamViews[i].teamNb == teamNb)
            {
                return teamViews[i].connectionId;
            }
        }
        return -1;
    }

    public TeamView connectionIdToTeamView(int connectionId)
    {
        int nb = teamViews.Count;
        for (int i = 0; i < nb; i++)
        {
            if (teamViews[i].connectionId == connectionId && teamViews[i].connectionId != -1)
            {
                return teamViews[i];
            }
        }
        return null;
    }

    public List<int> GetActiveConnectionIds()
    {
        List<int> ids = new List<int>();
        for (int i = 0; i < teamViews.Count; i++)
        {
            if (teamViews[i].connectionId >= 1)
                ids.Add(teamViews[i].connectionId);
        }
        return ids;
    }

    public void OnKick(int teamNb)
    {
        TeamView view = teamViews.Find(v => v.teamNb == teamNb);
        if (view != null)
        {
            view.connectionId = -1;
            view.isOccupied = false;
            view.totalScore = 0;
            TeamNumberToTeamView(teamNb).ActivateTeamSlot();
            joinedTeamNb--;

            AdminServerManagement.instance.OnChangeCurrentTeamNumber(joinedTeamNb);
        }
    }

    public void OnTeamFoundObject(int teamNb, int connectionId, int score, int nbItems)
    {
        TeamView view = TeamNumberToTeamView(teamNb);
        if (view != null)
        {
            view.UpdateScore(score, UpdateType.overRide);
            view.UpdateItemsFound(nbItems);
        }
    }

    public bool canJoin(int teamNB)
    {
        if (slotOccupied(teamNB))
        {
            return false;
        }

        return true;
    }

    public bool slotOccupied(int teamNb)
    {
        TeamView view = TeamNumberToTeamView(teamNb);
        if (view != null)
        {
            if (view.isOccupied)
            {
                return true;
            }
        }
        return false;
    }

    public List<int> AvailableTeamsList()
    {
        List<int> teamNbs = new List<int>();

        for (int i = 0; i < teamViews.Count; i++)
        {
            if (!teamViews[i].isOccupied)
            {
                teamNbs.Add(teamViews[i].teamNb);
            }
        }

        return teamNbs;
    }
}