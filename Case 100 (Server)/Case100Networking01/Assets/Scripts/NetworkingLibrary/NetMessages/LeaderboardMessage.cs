using System.Collections;
using CaseNetworking;

[System.Serializable]
public class LeaderboardMessage : CaseNetMessage
{
    public Hashtable ht;

    public LeaderboardMessage()
    {
        messageCode = MessageCode.LeaderboardMessage;
    }
}