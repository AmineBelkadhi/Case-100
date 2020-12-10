using CaseNetworking;

[System.Serializable]
public class SendScoreMessage : CaseNetMessage
{
    public int value;

    public SendScoreMessage()
    {
        messageCode = MessageCode.SendScoreMessage;
    }
}