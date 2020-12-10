using CaseNetworking;

[System.Serializable]
public class PauseGameMessage : CaseNetMessage
{
    public PauseGameMessage()
    {
        messageCode = MessageCode.PauseGameMessage;
    }
}