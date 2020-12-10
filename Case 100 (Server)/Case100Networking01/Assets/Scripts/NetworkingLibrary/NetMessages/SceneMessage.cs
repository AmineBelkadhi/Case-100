using CaseNetworking;

[System.Serializable]
public class SceneMessage : CaseNetMessage
{
    public int value;

    public SceneMessage()
    {
        messageCode = MessageCode.SceneMessage;
    }
}