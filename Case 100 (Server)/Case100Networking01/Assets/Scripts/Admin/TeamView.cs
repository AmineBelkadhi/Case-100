using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CaseNetworking;

public enum UpdateType
{
    overRide, add, remove
};

public class TeamView : MonoBehaviour
{
    public int teamNb;
    public int connectionId = -1;

    [HideInInspector]
    public bool isOccupied = false;

    [Header("Team View Panel")]
    [SerializeField]
    public GameObject teamViewPanel;

    [SerializeField]
    private TextMeshProUGUI teamNbText;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI itemsText;

    [Header("Team Slot Panel")]
    [SerializeField]
    public GameObject teamSlotPanel;

    [SerializeField]
    private TextMeshProUGUI slotText;

    [SerializeField]
    public int slotNb;

    [Header("Buttons")]
    public Button addScoreButton;

    public Button removeScoreButton;
    public Button kickTeamButton;
    public Button removeSlotButton;

    [Header("Progressions")]
    public int totalScore = 0;

    public int nbItemsFound = 0;

    private void Awake()
    {
        addScoreButton.onClick.AddListener(() => AddScore());
        removeScoreButton.onClick.AddListener(() => RemoveScore());
        removeSlotButton.onClick.AddListener(() => TeamManagement.instance.DeleteTeamSlot(slotNb));
        kickTeamButton.onClick.AddListener(() => KickTeam());
    }

    public void AddScore()
    {
        Debug.Log("Add score to " + teamNb);

        TeamManagement.instance.addScoreView.EnableScoreView(teamNb, connectionId);
    }

    public void RemoveScore()
    {
        Debug.Log("Remove  score from " + teamNb);
        TeamManagement.instance.removeScoreView.EnableScoreView(teamNb, connectionId);
    }

    public void KickTeam()
    {
        if (connectionId >= 0)
        {
            // ActivateTeamSlot();
            TeamManagement.instance.kickTeamView.EnableKickPanel(teamNb, connectionId);
        }
        Debug.Log("Send Message to remove team " + teamNb);
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void SetItems(int nbItems)
    {
        itemsText.text = nbItems.ToString();
    }

    public void Init(int _teamNb)
    {
        SetSlot(_teamNb);
        teamNb = _teamNb;
        ActivateTeamSlot();
        teamNbText.text = "TEAM " + _teamNb;
        SetScore(0);
        SetItems(0);
    }

    public void ActivateTeamView()
    {
        teamViewPanel.SetActive(true);
        teamSlotPanel.SetActive(false);
    }

    public void ActivateTeamSlot()
    {
        teamViewPanel.SetActive(false);
        teamSlotPanel.SetActive(true);
        connectionId = -1;
    }

    public void SetSlot(int slot)
    {
        slotNb = slot;
        slotText.text = slot.ToString();
    }

    public void UpdateScore(int score, UpdateType updateType)
    {
        if (updateType == UpdateType.overRide)
        {
            totalScore = score;
        }
        else if (updateType == UpdateType.add)
        {
            totalScore += score;
        }
        else if (updateType == UpdateType.remove)
        {
            if (totalScore - score >= 0)
                totalScore -= score;
            else
                totalScore = 0;
        }
        scoreText.text = totalScore.ToString();
    }

    public void UpdateItemsFound(int nb)
    {
        nbItemsFound++;
        itemsText.text = nbItemsFound.ToString();
    }
}