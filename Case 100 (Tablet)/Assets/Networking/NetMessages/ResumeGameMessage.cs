using CaseNetworking;

[System.Serializable]
public class ResumeGameMessage : CaseNetMessage
{
    public ResumeGameMessage()
    {
        messageCode = MessageCode.ResumeGameMessage;
    }
}