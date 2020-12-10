using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

namespace CaseNetworking
{
    public class CaseClient : MonoBehaviour
    {
        public delegate void ConnectionsHandler(int id);
        public event ConnectionsHandler ConnectEvent;
        public event ConnectionsHandler DisconnectEvent;

        public delegate void DataHandler(int recHostID, int connectionID, int channelID, CaseNetMessage msg);
        public event DataHandler OnDataEvent;

        private byte reliableChannel;
        private byte unreliableChannel;

        public int hostID;
        public int connectionID;

        public bool clientRunning;
        public bool hasConnected;

        private byte error;

        public void StartClient()
        {
            if (!NetworkTransport.IsStarted)
            {
                NetworkTransport.Init();
            }

            if (clientRunning)
            {
                Debug.Log("Cliebt already running ");
            }
            Debug.Log("Client Starting");

            ConnectionConfig config = new ConnectionConfig();

            reliableChannel = config.AddChannel(QosType.Reliable);
            unreliableChannel = config.AddChannel(QosType.Unreliable);

            HostTopology topology = new HostTopology(config, 100);

            hostID = NetworkTransport.AddHost(topology, 0);

            if (hostID != -1)
            {
                Debug.Log("Client Started");
                clientRunning = true;
            }
        }

        public bool Connect(string ipAdress, int port)
        {
            connectionID = NetworkTransport.Connect(hostID, ipAdress, port, 0, out error);
            if (connectionID < 1)
            {
                Debug.Log("failed to connect");
                return false;
            }
            hasConnected = true;
            return true;
        }

        private void Update()
        {
            if (!clientRunning)
                return;

            int recHostId;
            int connectionId;
            int channelId;

            byte[] recBuffer = new byte[1024];
            int dataSize;
            byte error;

            NetworkEventType type = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer,
              1024, out dataSize, out error);

            switch (type)
            {
                case NetworkEventType.Nothing:
                    break;

                case NetworkEventType.ConnectEvent:
                    //Debug.Log("TEAM CONNECT TO SERVER :D");
                    if (ConnectEvent != null)
                    {
                        ConnectEvent(connectionId);
                    }
                    else
                    {
                        Debug.Log("clientConnectEvent is empty");
                    }
                    break;

                case NetworkEventType.DisconnectEvent:
                    Debug.Log("SERVER WENT DOWN :O");
                    if (DisconnectEvent != null)
                    {
                        DisconnectEvent(connectionId);
                    }
                    else
                    {
                        Debug.Log("Client disconnect event is empty");
                    }
                    break;

                case NetworkEventType.DataEvent:
                    Debug.Log("Server  Sent me this shit ");
                    BinaryFormatter formatter = new BinaryFormatter();
                    MemoryStream stream = new MemoryStream(recBuffer);
                    CaseNetMessage msg = (CaseNetMessage)formatter.Deserialize(stream);

                    if (OnDataEvent != null)
                    {
                        OnDataEvent(recHostId, connectionId, channelId, msg);
                    }
                    else
                    {
                        Debug.Log("OnDataEvent  is empty");
                    }
                    break;

                default:
                case NetworkEventType.BroadcastEvent:
                    // Debug.Log("Default broadcast");
                    break;
            }
        }

        public void SendMessageToServer(CaseNetMessage msg)
        {
            byte error;
            byte[] buffer = new byte[CaseNet_Config.MAX_BYTE_SIZE];

            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(buffer);
            formatter.Serialize(stream, msg);

            NetworkTransport.Send(hostID, connectionID, reliableChannel, buffer, CaseNet_Config.MAX_BYTE_SIZE, out error);
        }

        public virtual void OnDataReceivedDefault()
        {
            Debug.Log("Received trash :dd");
        }

        public void Disconnect()
        {
            NetworkTransport.RemoveHost(hostID);
        }
    }
}