using CaseNetworking;

[System.Serializable]
public class FoundItemMessage : CaseNetMessage
{
    public int itemId;
    public int score;

    public FoundItemMessage()
    {
        messageCode = MessageCode.FoundItemMessage;
    }
}