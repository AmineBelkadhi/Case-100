using CaseNetworking;

[System.Serializable]
public class RemoveTimeMessage : CaseNetMessage
{
    public int value;

    public RemoveTimeMessage()
    {
        messageCode = MessageCode.RemoveTimeMessage;
    }
}