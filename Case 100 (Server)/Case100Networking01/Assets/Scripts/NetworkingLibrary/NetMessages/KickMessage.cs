using CaseNetworking;

[System.Serializable]
public class KickMessage : CaseNetMessage
{
    public KickMessage()
    {
        messageCode = MessageCode.KickMessage;
    }
}