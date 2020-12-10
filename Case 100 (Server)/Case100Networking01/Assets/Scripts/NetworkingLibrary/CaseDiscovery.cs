using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

namespace CaseNetworking
{
    public class CaseDiscovery : MonoBehaviour
    {
        public delegate void DiscoveryHandler(string ip, int port);
        public event DiscoveryHandler OnDiscover;

        public bool isDiscovering;

        public int discoveryHostID;
        private HostTopology topology;
        private byte unreliableChannel;

        private byte[] msgBuffer;

        //private void Start()
        //{
        //    StartDiscovering(CaseNet_Config.broadcastPort, CaseNet_Config.broadcastKey,
        //    CaseNet_Config.broadcastVersion, CaseNet_Config.broadcastSubVersion);
        //}

        public bool StartDiscovering(int broadcastPort, int broadcastKey, int broadcastVersion, int broadcastSubVersion)
        {
            if (!NetworkTransport.IsStarted)
            {
                NetworkTransport.Init();
            }

            msgBuffer = new byte[1024];
            ConnectionConfig config = new ConnectionConfig();
            unreliableChannel = config.AddChannel(QosType.Unreliable);
            topology = new HostTopology(config, 1);

            Debug.Log("Discovery initiliazed...");

            if (isDiscovering)
            {
                Debug.Log("Client already discovering");
                return false;
            }

            discoveryHostID = NetworkTransport.AddHost(topology, broadcastPort);

            if (discoveryHostID == -1)
            {
                Debug.Log("Failed AddHost");

                return false;
            }

            byte error;
            //OnDiscover += OnBroadcastdiscovery;
            NetworkTransport.SetBroadcastCredentials(discoveryHostID, broadcastKey, broadcastVersion,
                broadcastSubVersion, out error);

            isDiscovering = true;
            Debug.Log("Started Discovering");

            Debug.Log(isDiscovering + "  " + discoveryHostID);
            return true;
        }

        private void Update()
        {
            if (discoveryHostID == -1 || !isDiscovering)
            {
                return;
            }

            Discover();
        }

        private void Discover()
        {
            NetworkEventType type;

            do
            {
                int recSize;
                int connectionId;
                int channelId;
                //int rechostID;
                byte error;
                byte[] recBuffer = new byte[1024];

                type = NetworkTransport.ReceiveFromHost(discoveryHostID, out connectionId, out channelId, msgBuffer,
                    1024, out recSize, out error);

                //type = NetworkTransport.Receive(out hostID, out connectionId,out channelId ,recBuffer, 1024,
                // out recSize, out error);

                if (type == NetworkEventType.BroadcastEvent)
                {
                    int serverPort;
                    byte brError;
                    string serverAdress;
                    NetworkTransport.GetBroadcastConnectionInfo(discoveryHostID, out serverAdress, out serverPort, out brError);

                    //OnBroadcastdiscovery(serverAdress, serverPort);
                    if (OnDiscover != null)
                    {
                        OnDiscover(serverAdress, serverPort);
                    }
                    else
                    {
                        Debug.Log("OnDiscover event empty");
                    }
                }
            } while (type != NetworkEventType.Nothing);
        }

        public void StopDiscovery()
        {
            if (discoveryHostID == -1)
            {
                Debug.Log("NetworkTransport not initialized");
            }

            if (!isDiscovering)
            {
                Debug.Log("Not Discovering, can't stop");
                return;
            }
            OnDiscover = null;
            Debug.Log("Stopped Discovering");
            NetworkTransport.RemoveHost(discoveryHostID);
            isDiscovering = false;
        }

        //public IEnumerator StartDiscover(int broadcastPort, int broadcastKey, int broadcastVersion, int broadcastSubVersion)
        //{
        //    if (discoveryHostID == -1 || !isDiscovering)
        //    {
        //        yield break;
        //    }
        //    Discover();
        //    yield return null;
        //}

        //public bool StartDiscover(int broadcastPort, int broadcastKey, int broadcastVersion, int broadcastSubVersion)
        //{
        //    if (!NetworkTransport.IsStarted)
        //    {
        //        NetworkTransport.Init();
        //    }

        //    msgBuffer = new byte[1024];
        //    ConnectionConfig config = new ConnectionConfig();
        //    unreliableChannel = config.AddChannel(QosType.Unreliable);
        //    topology = new HostTopology(config, 1);

        //    Debug.Log("Discovery initiliazed...");

        //    if (isDiscovering)
        //    {
        //        Debug.Log("Client already discovering");
        //        return false;
        //    }

        //    discoveryHostID = NetworkTransport.AddHost(topology, broadcastPort);

        //    if (discoveryHostID == -1)
        //    {
        //        Debug.Log("Failed AddHost");

        //        return false;
        //    }

        //    byte error;

        //    NetworkTransport.SetBroadcastCredentials(discoveryHostID, broadcastKey, broadcastVersion,
        //        broadcastSubVersion, out error);

        //    Debug.Log(discoveryHostID);

        //    isDiscovering = true;
        //    Debug.Log("Started Discovering");

        //    while (isDiscovering && discoveryHostID != -1)
        //    {
        //        Discover();
        //    }
        //    return true;
        //}

        //public void BeginDiscovery(int broadcastPort, int broadcastKey, int broadcastVersion, int broadcastSubVersion)
        //{
        //}
    }
}