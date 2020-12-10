using CaseNetworking;

[System.Serializable]
public class RemoveScoreMessage : CaseNetMessage
{
    public int value;

    public RemoveScoreMessage()
    {
        messageCode = MessageCode.RemoveScoreMessage;
    }
}