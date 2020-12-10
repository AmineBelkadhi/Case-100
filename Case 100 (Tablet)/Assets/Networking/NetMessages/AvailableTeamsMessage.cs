using System.Collections.Generic;
using CaseNetworking;

[System.Serializable]
public class AvailableTeamsMessage : CaseNetMessage
{
    public List<int> availableTeams = new List<int>();

    public AvailableTeamsMessage()
    {
        messageCode = MessageCode.AvailableTeamsMessage;
    }
}