using CaseNetworking;

[System.Serializable]
public class ItemMessage : CaseNetMessage
{
    public int value;

    public ItemMessage()
    {
        messageCode = MessageCode.ItemMessage;
    }
}