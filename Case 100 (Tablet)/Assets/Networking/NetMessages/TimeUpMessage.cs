using CaseNetworking;

[System.Serializable]
public class TimeUpMessage : CaseNetMessage
{
    public TimeUpMessage()
    {
        messageCode = MessageCode.TimeUpMessage;
    }
}