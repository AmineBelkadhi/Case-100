using CaseNetworking;

[System.Serializable]
public class AddTimeMessage : CaseNetMessage
{
    public int value;

    public AddTimeMessage()
    {
        messageCode = MessageCode.AddTimeMessage;
    }
}