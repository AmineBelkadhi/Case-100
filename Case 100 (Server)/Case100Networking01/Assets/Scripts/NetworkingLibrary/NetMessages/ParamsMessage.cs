using System.Collections.Generic;
using CaseNetworking;

[System.Serializable]
public class ParamsMessage : CaseNetMessage
{
    public int sceneId;
    public List<int> itemIds;
    public int timePerItem;
    public int scorePerItem;

    public ParamsMessage()
    {
        messageCode = MessageCode.ParamsMessage;
    }
}