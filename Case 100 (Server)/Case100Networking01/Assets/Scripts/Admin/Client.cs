using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaseNetworking;
using UnityEngine.Networking;
using TMPro;

public class Client : CaseClient
{
    public CaseDiscovery discovery;

    public TMP_InputField teamNbField;

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

        discovery.OnDiscover += OnDiscovery;
        //StartCoroutine(testMessage(x));
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
        Debug.Log(ipAdress);
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
        SendTeamJoinMessage(int.Parse(teamNbField.text));
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
            Debug.Log(string.Format("nb of teams : {0} \n scene Id : {1} \n ",
                 parMsg.sceneId, parMsg.timePerItem));

            for (int i = 0; i < parMsg.itemIds.Count; i++)
            {
                Debug.Log(parMsg.itemIds[i]);
            }
        }
    }

    public void OnReceiveStartGameMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.StartGameMessage)
        {
            Debug.Log("Start game message received");

            // start game
        }
    }

    public void OnReceiveAddScoreMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.SendScoreMessage)
        {
            SendScoreMessage scoreMsg = (SendScoreMessage)msg;
            Debug.Log("Received score == " + scoreMsg.value);
        }
    }

    public void OnReceiveRemoveScoreMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.RemoveScoreMessage)
        {
            RemoveScoreMessage scoreMsg = (RemoveScoreMessage)msg;
            Debug.Log("Removed score == " + scoreMsg.value);
        }
    }

    public void OnReceiveKickMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.KickMessage)
        {
            Debug.Log("Kicked by server :'(");
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
            }
            else
            {
                Debug.Log(string.Format("can't join \n Consider joining these teams {0}", respMsg.availableTeams.Count));
                //refresh the available teams from respMsg.availableTeams
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
        }
    }

    public void OnReceiveAddTimeMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.AddTimeMessage)
        {
            AddTimeMessage timeMsg = (AddTimeMessage)msg;

            Debug.Log("Time added : " + timeMsg.value);
        }
    }

    public void OnReceiveRemoveTimeMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.RemoveTimeMessage)
        {
            RemoveTimeMessage timeMsg = (RemoveTimeMessage)msg;
            Debug.Log("Time removed : " + timeMsg.value);
        }
    }

    public void OnReceiveGameOverMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.GameOverMessage)
        {
            Debug.Log("Game Over, Pack it up homie :'(");
        }
    }

    public void OnReceiveTimeUpMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.TimeUpMessage)
        {
            Debug.Log("Time Up Homie");
        }
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