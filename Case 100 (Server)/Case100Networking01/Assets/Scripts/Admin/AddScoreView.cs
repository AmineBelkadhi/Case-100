using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CaseNetworking;

public class AddScoreView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI teamNbText;

    [SerializeField]
    private TMP_InputField scoreValueField;

    [SerializeField]
    private Button addScoreButton;

    public void EnableScoreView(int teamNb, int connectionId)
    {
        if (!OperationPanelLock.operationPanelOn)
        {
            teamNbText.text = teamNb.ToString();
            gameObject.SetActive(true);
            addScoreButton.onClick.RemoveAllListeners();
            addScoreButton.onClick.AddListener(() => OnAddScore(teamNb, connectionId, UpdateType.add));
            OperationPanelLock.OnOpen();
        }
    }

    public void DisableAddScoreView()
    {
        gameObject.SetActive(false);
        OperationPanelLock.OnClose();
    }

    public void OnAddScore(int teamNb, int connectionId, UpdateType updateType)
    {
        int value = int.Parse(scoreValueField.text);
        if (value <= 0)
        {
            Debug.Log("You're trying to add 0 or negative score homie");
        }
        else
        {
            Debug.Log(string.Format("Checking then sending team {0} {1} score",
             teamNb, value));
            gameObject.SetActive(false);
            Server.instance.SendAddScoreMessage(connectionId, value);
            InfoTextView.instance.SetInfo(string.Format("Added {0} score to team {1}",
             teamNb, value));
            OperationPanelLock.OnClose();

            TeamView view = TeamManagement.instance.TeamNumberToTeamView(teamNb);
            if (view != null)
            {
                view.UpdateScore(value, updateType);
            }

            CaseLogs.instance.AddToLog(string.Format("Added {0} score to team {1}", value, teamNb));

            scoreValueField.text = "";
        }
    }
}