using CaseNetworking;

[System.Serializable]
public class StopGameMessage : CaseNetMessage
{
    public StopGameMessage()
    {
        messageCode = MessageCode.StopGameMessage;
    }
}