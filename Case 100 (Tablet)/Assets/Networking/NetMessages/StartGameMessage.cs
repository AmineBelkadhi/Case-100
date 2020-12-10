using CaseNetworking;

[System.Serializable]
public class StartGameMessage : CaseNetMessage
{
    public StartGameMessage()
    {
        messageCode = MessageCode.StartGameMessage;
    }
}