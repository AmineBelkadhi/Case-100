namespace CaseNetworking
{
    [System.Serializable]
    public class CaseNetMessage
    {
        public byte messageCode { get; set; }

        public CaseNetMessage()
        {
            messageCode = MessageCode.none;
        }
    }
}