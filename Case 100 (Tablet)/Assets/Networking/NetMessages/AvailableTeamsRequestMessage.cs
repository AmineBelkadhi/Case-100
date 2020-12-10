using System.Collections.Generic;
using CaseNetworking;

[System.Serializable]
public class AvailableTeamsRequestMessage : CaseNetMessage
{
    public AvailableTeamsRequestMessage()
    {
        messageCode = MessageCode.AvailableTeamsRequestMessage;
    }
}