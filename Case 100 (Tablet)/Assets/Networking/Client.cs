using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaseNetworking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Client : CaseClient
{
    
    public static Client instance;

    public bool paused= false;
    public Text scoreText;
    public GameObject gameOverPanel;
    public GameObject kickedPanel;
    public GameObject pausedPanel;
    public int idItem = -1;

    public CaseDiscovery discovery;

    public ServerMenu svMenu;
    public List<string> serverIps= new List<string>();

    public TeamMenu teamMenu;
    public GameObject waitingScreen;
    public ScoreManager scoreManager;

    public StartTheGame startTheGame;
    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        OnDataEvent += OnReceiveParams;
        OnDataEvent += OnReceiveJoinResponse;
        OnDataEvent += OnReceiveStartGameMessage;
        OnDataEvent += OnReceiveRemoveScoreMessage;
        OnDataEvent += OnReceiveAddScoreMessage;
        OnDataEvent += OnReceiveTimeUpMessage;
        OnDataEvent += OnReceiveGameOverMessage;
        OnDataEvent += OnReceiveAddTimeMessage;
        OnDataEvent += OnReceiveRemoveTimeMessage;
        OnDataEvent += OnReceiveLeaderboardsMessage;
        OnDataEvent += OnReceiveKickMessage;
        OnDataEvent += OnReceiveAvailableTeamsMessage;
        OnDataEvent += OnReceivePauseMessage;
        OnDataEvent += OnReceiveItemMessage;
        OnDataEvent += OnReceiveSceneMessage;
        OnDataEvent += OnReceiveResumeMessage;

        discovery.OnDiscover += OnDiscovery;
        //StartCoroutine(testMessage(x));

        StartClient();
    }

    public void Test(int x)
    {
        StartCoroutine(testMessage(x));
    }

    public void Send()
    {
        SendFoundItemMessage(9, 500);
        SendJoinRequestMessage(88);
        SendTeamJoinMessage(69);
    }

    public void StartDiscovery()
    {
        discovery.StartDiscovering(CaseNet_Config.broadcastPort, CaseNet_Config.broadcastKey,
            CaseNet_Config.broadcastVersion, CaseNet_Config.broadcastSubVersion);
    }

    public void AttemptConnection(string ipAdress, int port)
    {
        if (Connect(ipAdress, CaseNet_Config.gamePort))
        {
            Debug.Log("connected to " + ipAdress);
        }
        else
        {
            Debug.Log("Failed to connect");
        }
    }

    private void OnDiscovery(string ipAdress, int port)
    {
        if (!serverIps.Contains(ipAdress))
        {
            serverIps.Add(ipAdress);
            Debug.Log(ipAdress);
            svMenu.instantiateServerButton(ipAdress);
        }
    }

    private IEnumerator testor(string ip)
    {
        yield return new WaitForSeconds(2f);
        Connect("192.168.1.237", CaseNet_Config.gamePort);
    }

    private IEnumerator testMessage(int x)
    {
        StartClient();
        yield return new WaitForSeconds(1f);
        Connect("192.168.1.237", CaseNet_Config.gamePort);
        yield return new WaitForSeconds(1f);
        
        yield return new WaitForSeconds(1f);
        SendFoundItemMessage(3, 150);
    }

    #region communication

    #region Receiving

    public void OnReceiveParams(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.ParamsMessage)
        {
            ParamsMessage parMsg = (ParamsMessage)msg;
           
            ScenesManager.instance.AddScene(parMsg.sceneId, parMsg.itemIds, parMsg.timePerItem, parMsg.scorePerItem);
            
        }
    }

    public void OnReceivePauseMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.PauseGameMessage)
        {
            paused = true;
            PauseGameMessage parMsg = (PauseGameMessage)msg;

            Debug.Log("Pause Homie");
            pausedPanel.gameObject.SetActive(true);

        }
    }
    public void OnReceiveResumeMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.ResumeGameMessage)
        {
            paused = false;
            ResumeGameMessage parMsg = (ResumeGameMessage)msg;

            Debug.Log("Resume Homie");
            pausedPanel.gameObject.SetActive(false);

        }
    }

    public void OnReceiveStartGameMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.StartGameMessage)
        {
            Debug.Log("Start game message received");

            // start game
            startTheGame.OnClickStartTheGame();
            waitingScreen.gameObject.SetActive(false);
        }
    }

    public void OnReceiveAddScoreMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.SendScoreMessage)
        {
            SendScoreMessage scoreMsg = (SendScoreMessage)msg;
            Debug.Log("Received score == " + scoreMsg.value);
            scoreManager.AddScore(scoreMsg.value);
        }
    }

    public void OnReceiveRemoveScoreMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.RemoveScoreMessage)
        {
            RemoveScoreMessage scoreMsg = (RemoveScoreMessage)msg;
            Debug.Log("Removed score == " + scoreMsg.value);
            scoreManager.RemoveScore(scoreMsg.value);
        }
    }

    public void OnReceiveKickMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.KickMessage)
        {
            Debug.Log("Kicked by server :'(");
           // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            kickedPanel.gameObject.SetActive(true);


        }
    }

    public void OnReceiveJoinResponse(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.JoinTeamResponseMessage)
        {
            JoinTeamResponseMessage respMsg = (JoinTeamResponseMessage)msg;

            if (respMsg.canJoin)
            {
                Debug.Log("Can join ALRIGHT");
                SendTeamJoinMessage(respMsg.teamNb);
                waitingScreen.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log(string.Format("can't join \n Consider joining these teams {0}", respMsg.availableTeams.Count));
                //refresh the available teams from respMsg.availableTeams
                teamMenu.ClearButtons();
                teamMenu.instantiateTeamButtons(respMsg.availableTeams);
            }
        }
    }

    public void OnReceiveLeaderboardsMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.LeaderboardMessage)
        {
            Debug.Log("Received a hashtable with leaderboards Inside");

            LeaderboardMessage leadMsg = (LeaderboardMessage)msg;
            foreach (DictionaryEntry t in leadMsg.ht)
            {
                Debug.Log("key : " + t.Key);
                Debug.Log("value: " + t.Value);
            }
        }
    }

    public void OnReceiveAvailableTeamsMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.AvailableTeamsMessage)
        {
            AvailableTeamsMessage tMsg = (AvailableTeamsMessage)msg;
            Debug.Log(string.Format("You have to Spawn {0} buttons", tMsg.availableTeams.Count));
            teamMenu.instantiateTeamButtons(tMsg.availableTeams);

        }
    }

    public void OnReceiveAddTimeMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.AddTimeMessage)
        {
            AddTimeMessage timeMsg = (AddTimeMessage)msg;

            Debug.Log("Time added : " + timeMsg.value);
            CountDown.instance.currentTime += timeMsg.value;
        }
    }

    public void OnReceiveRemoveTimeMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.RemoveTimeMessage)
        {
            RemoveTimeMessage timeMsg = (RemoveTimeMessage)msg;
            Debug.Log("Time removed : " + timeMsg.value);
            CountDown.instance.currentTime += timeMsg.value;
        }
    }

    public void OnReceiveGameOverMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.GameOverMessage)
        {
            Debug.Log("Game Over, Pack it up homie :'(");
            gameOverPanel.gameObject.SetActive(true);
            scoreText.text = string.Format("Score: {0}", scoreManager.score);
            

        }
    }

    public void OnReceiveTimeUpMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.TimeUpMessage)
        {
            Debug.Log("Time Up Homie");

        }
    }
    public void OnReceiveSceneMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.SceneMessage)
        {
            SceneMessage sceneMsg = (SceneMessage)msg;
            ScenesManager.instance.ActivateScene(sceneMsg.value);
            //startTheGame.currentSceneIndex++;
            //startTheGame.StartScene();
            Debug.Log(sceneMsg.value);
        }
    }

    public void OnReceiveItemMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.ItemMessage)
        {
            ItemMessage itemMsg = (ItemMessage)msg;
            Debug.Log(itemMsg.value);
            idItem = itemMsg.value;
            startTheGame.ItemToFind(itemMsg.value);

            
        }
    }
    public void DisableKickedPanel()
    {
        kickedPanel.gameObject.SetActive(false);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool PausedPanelExist() //verifier si pausedpanel est active dans l'hierarchie
    {
        if (pausedPanel.gameObject.activeInHierarchy == false)
        {
            return false;
        }
        else
            return true;
    }

    #endregion Receiving

    #region Sending

    public void SendJoinRequestMessage(int teamNb)
    {
        JoinTeamRequestMessage reqMsg = new JoinTeamRequestMessage();
        reqMsg.teamNb = teamNb;

        SendMessageToServer(reqMsg);
    }

    public void SendFoundItemMessage(int itemId, int score)
    {
        FoundItemMessage itMsg = new FoundItemMessage();
        itMsg.itemId = itemId;
        itMsg.score = score;

        SendMessageToServer(itMsg);
    }

    public void SendTeamJoinMessage(int teamNb)
    {
        TeamNbMessage nbMsg = new TeamNbMessage();
        nbMsg.teamNb = teamNb;
        SendMessageToServer(nbMsg);
    }

    public void SendImScreenMessage()
    {
        ImScreenMessage nbMsg = new ImScreenMessage();

        SendMessageToServer(nbMsg);
    }

    #endregion Sending

    #endregion communication
}