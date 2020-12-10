using UnityEngine;
using UnityEngine.Networking;

namespace CaseNetworking
{
    public class CaseNetBroadcast
    {
        public int broadcastHostID;
        public bool isBroadcasting;

        public byte unreliableChannel;
        private HostTopology topology;

        public void StartBroadcast(int broadcastPort, int broadcastKey, int broadcastVersion, int broadcastSubVersion)
        {
            if (!NetworkTransport.IsStarted)
            {
                NetworkTransport.Init();
            }

            ConnectionConfig config = new ConnectionConfig();
            unreliableChannel = config.AddChannel(QosType.Unreliable);
            topology = new HostTopology(config, 1);
            Debug.Log("setting up topology and config");

            if (isBroadcasting)
            {
                Debug.Log("Already Broadcasting");
                return;
            }

            broadcastHostID = NetworkTransport.AddHost(topology, 10);
            if (broadcastHostID == -1)
            {
                Debug.Log("Failed to add a host and open a socket");
                return;
            }

            byte error;
            byte[] buffer = new byte[1024];

            if (!NetworkTransport.StartBroadcastDiscovery(broadcastHostID, CaseNet_Config.broadcastPort, CaseNet_Config.broadcastKey,
                CaseNet_Config.broadcastVersion, CaseNet_Config.broadcastSubVersion, buffer, 1024, 500, out error))
            {
                Debug.Log("Failed to broadcast");
                return;
            }

            Debug.Log("Broadcast Success");

            isBroadcasting = true;
        }

        public void StopBroadcast()
        {
            if (broadcastHostID == -1)
            {
                Debug.Log("NetworkTransport not initialized");
            }

            if (!isBroadcasting)
            {
                Debug.Log("Not broadcasting, can't stop");
                return;
            }

            Debug.Log("Stopped broadcast");
            NetworkTransport.StopBroadcastDiscovery();
            NetworkTransport.RemoveHost(broadcastHostID);
            isBroadcasting = false;
        }
    }
}