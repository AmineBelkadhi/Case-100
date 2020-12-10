using CaseNetworking;

[System.Serializable]
public class ImScreenMessage : CaseNetMessage
{
    public ImScreenMessage()
    {
        messageCode = MessageCode.ImScreenMessage;
    }
}