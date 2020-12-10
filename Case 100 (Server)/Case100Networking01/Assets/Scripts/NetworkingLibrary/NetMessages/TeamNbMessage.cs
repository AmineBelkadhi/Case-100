using CaseNetworking;

[System.Serializable]
public class TeamNbMessage : CaseNetMessage
{
    public int teamNb;

    public TeamNbMessage()
    {
        messageCode = MessageCode.TeamNbMessage;
    }
}