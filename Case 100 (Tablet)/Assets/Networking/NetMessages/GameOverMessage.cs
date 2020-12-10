using CaseNetworking;

[System.Serializable]
public class GameOverMessage : CaseNetMessage
{
    public GameOverMessage()
    {
        messageCode = MessageCode.GameOverMessage;
    }
}