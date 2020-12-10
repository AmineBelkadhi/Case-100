using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaseNetworking;

public class Server : CaseServer
{
    public CaseNetBroadcast broadcast = new CaseNetBroadcast();
    public Dictionary<int, int> teamConnId = new Dictionary<int, int>(); // key = team number, value = connectionId

    public static Server instance = null;

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        clientConnectEvent += OnClientConnected;
    }

    public void Start()
    {
        OnDataEvent += OnReceiveFoundItemMessage;
        OnDataEvent += OnReceiveTeamJoined;
        OnDataEvent += OnReceiveJoinTeamRequestMessage;
        OnDataEvent += OnReceiveScreenMessage;

        clientDisconnectionEvent += OnTeamDisconnect;
    }

    public void StartServer()
    {
        if (broadcast != null)
        {
            broadcast.StartBroadcast(CaseNet_Config.broadcastPort, CaseNet_Config.broadcastKey,
                CaseNet_Config.broadcastVersion, CaseNet_Config.broadcastSubVersion);
        }

        StartServer(CaseNet_Config.gamePort, CaseNet_Config.MAX_CONNEXIONS);
    }

    //test purpose
    public void Send()
    {
        //SendJoinResponseMessage(clientIds[0], true, new List<int>() { 1, 2, 3 });
        //SendJoinResponseMessage(clientIds[0], false, new List<int>() { 1, 2, 3 });
        SendStartGameMessage(clientIds);
        SendAddScoreMessage(clientIds[0], 69);
        SendRemoveScoreMessage(clientIds[0], 150000);
        SendTimeUpMessage(clientIds);
        SendGameOverMessage(clientIds);
        SendAddTimeMessage(clientIds, 20);
        SendRemoveTimeMessage(clientIds, 50);

        Hashtable hash = new Hashtable();
        hash.Add(1, 1500);
        hash.Add(2, 2500);
        hash.Add(8, 30);

        SendLeaderboards(clientIds[0], hash);
    }

    #region ConnectionsHandling

    //✓
    private void OnClientConnected(int connectionId)
    {
        clientIds.Add(connectionId);
        Debug.Log(string.Format("Team connected with an ID= {0}", connectionId));
        //   SendParametersToClient(connectionId, 1, 4, new List<int>() { 2, 3, 5 }, 15);
        SendAvailableTeamsMsg(connectionId);
        CaseLogs.instance.AddToLog("Client " + connectionId + " have connected");
    }

    //✓
    private void OnClientDisconnected(int connectionId)
    {
        clientIds.Remove(connectionId);
        Debug.Log("Team disconnected with an ID = " + connectionId);
        CaseLogs.instance.AddToLog("Client " + connectionId + " have disconnected");
    }

    #endregion ConnectionsHandling

    #region Communication Handling

    #region Sending

    //✓
    public void SendAvailableTeamsMsg(int id)
    {
        AvailableTeamsMessage msg = new AvailableTeamsMessage();
        msg.availableTeams = TeamManagement.instance.AvailableTeamsList();
        SendMessageToClient(id, reliableChannel, msg);
    }

    public void SendParametersToClient(int id, int sceneId, List<int> itemIds, int time, int score)
    {
        ParamsMessage msg = new ParamsMessage();

        msg.sceneId = sceneId;
        msg.itemIds = itemIds;
        msg.timePerItem = time;
        msg.scorePerItem = score;

        SendMessageToClient(id, reliableChannel, msg);
    }

    //✓
    public void SendJoinResponseMessage(int id, bool canJoin, int teamNb, List<int> availableTeams)
    {
        JoinTeamResponseMessage msg = new JoinTeamResponseMessage();
        msg.canJoin = canJoin;
        msg.teamNb = teamNb;
        msg.availableTeams = availableTeams;
        SendMessageToClient(id, reliableChannel, msg);
    }

    //✓
    public void SendStartGameMessage(List<int> ids)
    {
        StartGameMessage msg = new StartGameMessage();
        SendMessageToAllClients(ids, reliableChannel, msg);
    }

    //✓
    public void SendTimeUpMessage(List<int> ids)
    {
        TimeUpMessage msg = new TimeUpMessage();
        SendMessageToAllClients(ids, reliableChannel, msg);
    }

    //✓
    public void SendGameOverMessage(List<int> ids)
    {
        GameOverMessage msg = new GameOverMessage();
        SendMessageToAllClients(ids, reliableChannel, msg);
    }

    //✓
    public void SendAddScoreMessage(int connectionid, int value)
    {
        SendScoreMessage msg = new SendScoreMessage();
        msg.value = value;
        SendMessageToClient(connectionid, reliableChannel, msg);
    }

    //✓
    public void SendRemoveScoreMessage(int connectionid, int value)
    {
        RemoveScoreMessage msg = new RemoveScoreMessage();
        msg.value = value;
        SendMessageToClient(connectionid, reliableChannel, msg);
    }

    //✓
    public void SendAddTimeMessage(List<int> ids, int value)
    {
        AddTimeMessage msg = new AddTimeMessage();
        msg.value = value;
        SendMessageToAllClients(ids, reliableChannel, msg);
    }

    //✓
    public void SendRemoveTimeMessage(List<int> ids, int value)
    {
        RemoveTimeMessage msg = new RemoveTimeMessage();
        msg.value = value;
        SendMessageToAllClients(ids, reliableChannel, msg);
    }

    public void SendLeaderboards(int connectionId, Hashtable leaderboard)
    {
        LeaderboardMessage msg = new LeaderboardMessage();
        msg.ht = leaderboard;
        SendMessageToClient(connectionId, reliableChannel, msg);
    }

    public void SendKickMessage(int connectionId)
    {
        KickMessage msg = new KickMessage();
        SendMessageToClient(connectionId, reliableChannel, msg);
    }

    public void SendPauseMessage(List<int> ids)
    {
        PauseGameMessage msg = new PauseGameMessage();
        SendMessageToAllClients(ids, reliableChannel, msg);
    }

    public void SendStopGameMessage(List<int> ids)
    {
        StopGameMessage msg = new StopGameMessage();
        SendMessageToAllClients(ids, reliableChannel, msg);
    }

    public void SendResumeGameMessage(List<int> ids)
    {
        ResumeGameMessage msg = new ResumeGameMessage();
        SendMessageToAllClients(ids, reliableChannel, msg);
    }

    public void SendSceneMessage(List<int> ids, int sceneId)
    {
        SceneMessage msg = new SceneMessage();
        msg.value = sceneId;
        SendMessageToAllClients(ids, reliableChannel, msg);
    }

    public void SendItemMessage(List<int> ids, int itemId)
    {
        ItemMessage msg = new ItemMessage();
        msg.value = itemId;
        SendMessageToAllClients(ids, reliableChannel, msg);
        Debug.Log("Sent item ID " + itemId);
    }

    #endregion Sending

    #region Receiving

    private void OnReceiveJoinTeamRequestMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.JoinTeamRequestMessage)
        {
            JoinTeamRequestMessage reqMsg = (JoinTeamRequestMessage)msg;
            //logs
            // verify if the slot in the message available
            // calculate the free slots
            //send join response}

            //IF SLOT ABAILABLE
            if (!TeamManagement.instance.slotOccupied(reqMsg.teamNb))
            {
                // send response message with canJoin = true
                SendJoinResponseMessage(connectionID, true, reqMsg.teamNb, new List<int>() { });
                for (int i = 0; i < GameConfigManager.instance.gameData.selectedScenes.Count; i++)
                {
                    SendParametersToClient(connectionID, GameConfigManager.instance.gameData.selectedScenes[i].id,
                        GameConfigManager.instance.gameData.selectedScenes[i].selectedItemIds,
                        GameConfigManager.instance.gameData.selectedScenes[i].timePerItem,
                        GameConfigManager.instance.gameData.selectedScenes[i].scorePerItem
                        );
                }
            }
            else //if SLOT NOT AVAILABLE
            {
                SendJoinResponseMessage(connectionID, false, reqMsg.teamNb, TeamManagement.instance.AvailableTeamsList());
            }

            Debug.Log("Join request from " + connectionID);
            CaseLogs.instance.AddToLog("Client " + connectionID + " sent a team request to join team " + reqMsg.teamNb);
        }
    }

    private void OnReceiveTeamJoined(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.TeamNbMessage)
        {
            //logs
            //associate team nb with connectionId nbMsg.teamNb
            TeamNbMessage nbMsg = (TeamNbMessage)msg;

            TeamManagement.instance.OnTeamJoin(nbMsg.teamNb, connectionID);
        }
    }

    private void OnReceiveFoundItemMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.FoundItemMessage)
        {
            FoundItemMessage itMsg = (FoundItemMessage)msg;
            Debug.Log("team " + connectionID + " found an object");
            CaseLogs.instance.AddToLog("Team " + connectionID + " have found object ID: " + itMsg.itemId);

            TeamView view = TeamManagement.instance.connectionIdToTeamView(connectionID);
            if (view != null)
            {
                view.UpdateScore(itMsg.score, UpdateType.overRide);
                view.UpdateItemsFound(2);
            }
        }
    }

    private void OnReceiveScreenMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.ImScreenMessage)
        {
            ImScreenMessage itMsg = (ImScreenMessage)msg;
            Debug.Log("Screen");
            ScreenManagement.instance.OnScreenConnected(connectionID);
        }
    }

    private void OnReceiveAvailableTeamsRequestMessage(int recHostID, int connectionID, int channelID, CaseNetMessage msg)
    {
        if (msg.messageCode == MessageCode.AvailableTeamsRequestMessage)
        {
            SendAvailableTeamsMsg(connectionID);
        }
    }

    #endregion Receiving

    #endregion Communication Handling

    #region TeamsManagement

    public void InitializeTeams(int teamsNumber)
    {
        for (int i = 1; i <= teamsNumber; i++)
        {
            teamConnId.Add(i, -1);
        }

        AssignTeam(2, 2);
        AssignTeam(1, 1);
        AssignTeam(3, 5);
    }

    public void AddTeamSlot()
    {
        teamConnId.Add(teamConnId.Count + 1, -1);
    }

    public void AssignTeam(int teamNb, int connectionId)
    {
        if (teamConnId.ContainsKey(teamNb))
        {
            teamConnId[teamNb] = connectionId;
            // return true;
        }
        // return false;
    }

    public void RemoveTeam(int teamNb)
    {
        if (teamConnId.ContainsKey(teamNb))
        {
            teamConnId.Remove(teamNb);
            // return true;
        }
        // return false;
    }

    public List<int> AvailableTeams()
    {
        if (teamConnId.Count > 0)
        {
            List<int> teamNbs = new List<int>();
            foreach (var team in teamConnId)
            {
                if (team.Value < 0)
                {
                    teamNbs.Add(team.Key);
                }
            }
            return teamNbs;
        }
        return null;
    }

    public void FreeTeamSlot(int teamSlot)
    {
        if (teamConnId.ContainsKey(teamSlot))
        {
            teamConnId[teamSlot] = -1;
            // return true;
        }
        // return false;
    }

    public void DebugTeams()
    {
        foreach (var team in teamConnId)
        {
            Debug.Log(string.Format("Team number: {0} , Connection Id: {1}", team.Key, team.Value));
        }

        Debug.Log("Available teams");

        List<int> availableTeams = AvailableTeams();

        foreach (var team in availableTeams)
        {
            Debug.Log(team);
        }
    }

    private int ConnectionIdToTeamNb(int connectionId)
    {
        foreach (var team in teamConnId)
        {
            if (team.Value == connectionId)
            {
                return team.Key;
            }
        }
        return -2;
    }

    public void OnTeamDisconnect(int connId)
    {
        foreach (var team in teamConnId)
        {
            if (team.Value == connId)
            {
                teamConnId[team.Key] = -1;
                CaseLogs.instance.AddToLog(team.Key.ToString() + " have disconnected.");
            }
        }
    }

    #endregion TeamsManagement

    #region ButtonActions

    public void SendStartGameMessageButton()
    {
        SendStartGameMessage(clientIds);
    }

    public void KickButton(int connectionId)
    {
        SendKickMessage(connectionId);
        StartCoroutine(DisconnectClientAfterDelay(2f, connectionId));
    }

    private IEnumerator DisconnectClientAfterDelay(float delay, int connectionId)
    {
        yield return new WaitForSeconds(delay);
        DisconnectClient(connectionId);
    }

    #endregion ButtonActions
}