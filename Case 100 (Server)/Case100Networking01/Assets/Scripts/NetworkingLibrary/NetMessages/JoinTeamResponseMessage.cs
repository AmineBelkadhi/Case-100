using System.Collections.Generic;
using CaseNetworking;

[System.Serializable]
public class JoinTeamResponseMessage : CaseNetMessage
{
    public bool canJoin;
    public int teamNb;
    public List<int> availableTeams;

    public JoinTeamResponseMessage()
    {
        messageCode = MessageCode.JoinTeamResponseMessage;
    }
}