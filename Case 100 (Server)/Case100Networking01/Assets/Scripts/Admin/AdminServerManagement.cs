using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CaseNetworking;

public class AdminServerManagement : MonoBehaviour
{
    public static AdminServerManagement instance;

    [Header("Texts")]
    [SerializeField]
    private TextMeshProUGUI serverStateText;

    [SerializeField]
    private TextMeshProUGUI scenesText;

    [SerializeField]
    private TextMeshProUGUI teamsText;

    [SerializeField]
    private TextMeshProUGUI serverOnTimeText;

    [SerializeField]
    private TextMeshProUGUI currentJoinedTeamsText;

    [SerializeField]
    private TextMeshProUGUI currentSceneText;

    [Header("Button Sprites")]
    [SerializeField]
    private Sprite pauseImage;

    [SerializeField]
    private Sprite resumeImage;

    [Header("Buttons")]
    public Button pauseButton;

    public Timer timer;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        timer = new Timer(OnUpdateTimer);

        pauseButton.onClick.AddListener(() => PauseTheGame());
    }

    public void OnUpdateTimer()
    {
        serverOnTimeText.text = timer.CurrentTimeFormat();
    }

    public void PauseServer()
    {
        //Server.instance.SendPauseMessage(TeamManagement.instance.GetActiveConnectionIds());
    }

    public void StopServer()
    {
        Debug.Log("Stopping Server");
    }

    public void InitialSetup()
    {
        if (Server.instance.serverRunning)
        {
            serverStateText.text = "Online";
        }
        else
        {
            serverStateText.text = "Offline";
        }

        scenesText.text = "/" + GameConfigManager.instance.gameData.selectedScenes.Count.ToString();

        teamsText.text = "/" + GameConfigManager.instance.gameData.nbTeams.ToString();

        currentJoinedTeamsText.text = "0";
        currentSceneText.text = "0";

        serverOnTimeText.text = "00:00:00";

        StartCoroutine(timer.StartTimer());
    }

    public void PauseTheGame()
    {
        if (!timer.isPaused)
        {
            Server.instance.SendPauseMessage(TeamManagement.instance.GetActiveConnectionIds());
            Debug.Log("Game is Paused");
            InfoTextView.instance.SetInfo("The server is paused");
            timer.Pause();

            pauseButton.image.sprite = resumeImage;
            CaseLogs.instance.AddToLog("The game is paused.");

            //Pause the countdown
            TimeManagement.instance.countDown.Pause();
        }
        else
        {
            Server.instance.SendResumeGameMessage(TeamManagement.instance.GetActiveConnectionIds());
            InfoTextView.instance.SetInfo("Game is resumed");
            timer.Resume();
            pauseButton.image.sprite = pauseImage;
            CaseLogs.instance.AddToLog("The game is resumed.");

            TimeManagement.instance.countDown.Resume();
        }
    }

    public void StopTheGame()
    {
        Server.instance.SendStopGameMessage(TeamManagement.instance.GetActiveConnectionIds());
        Debug.Log("Stopping the game");
        InfoTextView.instance.SetInfo("Stopped the server");

        StartCoroutine(restart());
    }

    private IEnumerator restart()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnChangeTeams(int teamsNb)
    {
        teamsText.text = "/" + teamsNb.ToString();
    }

    public void OnChangeMaxScenes(int scenesNb)
    {
        scenesText.text = "/" + scenesNb.ToString();
    }

    public void OnChangeCurrentTeamNumber(int joinTeamsNb)
    {
        currentJoinedTeamsText.text = joinTeamsNb.ToString();
    }
}