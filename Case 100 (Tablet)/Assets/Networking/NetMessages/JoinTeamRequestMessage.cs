using CaseNetworking;

[System.Serializable]
public class JoinTeamRequestMessage : CaseNetMessage
{
    public int teamNb;

    public JoinTeamRequestMessage()
    {
        messageCode = MessageCode.JoinTeamRequestMessage;
    }
}