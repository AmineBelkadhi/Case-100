namespace CaseNetworking
{
    public class CaseConnection
    {
        public int connHostId;
        public int connConnectionId;

        public bool isConnected;

        public CaseConnection()
        {
            connHostId = -1;
            connConnectionId = -1;
            isConnected = false;
        }

        public CaseConnection(int hostId, int connectionId)
        {
            connHostId = hostId;
            connConnectionId = connectionId;
        }

        public CaseConnection(int hostId, int connectionId, bool _isConnected)
        {
            connHostId = hostId;
            connConnectionId = connectionId;
            isConnected = _isConnected;
        }
    }
}