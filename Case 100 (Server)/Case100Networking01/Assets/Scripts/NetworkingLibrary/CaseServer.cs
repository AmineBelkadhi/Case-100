using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

namespace CaseNetworking
{
    public class CaseServer : MonoBehaviour
    {
        //Server Events, called when the you receive a connect/disconnect/data messages
        public delegate void ConnectionsHandler(int id);
        public event ConnectionsHandler clientConnectEvent;
        public event ConnectionsHandler clientDisconnectionEvent;

        public delegate void DataHandler(int recHostID, int connectionID, int channelID, CaseNetMessage msg);
        public event DataHandler OnDataEvent;

        [HideInInspector]
        public byte reliableChannel;

        [HideInInspector]
        public byte unreliableChannel;

        private int hostID;

        public bool serverRunning;
        public bool listening;

        public List<int> clientIds = new List<int>();

        public List<CaseConnection> connections = new List<CaseConnection>();

        //public CaseNetBroadcast broadcast = new CaseNetBroadcast();

        public void StartServer(int port, int maxConnections)
        {
            //start the network transport in case it's not started already
            if (!NetworkTransport.IsStarted)
            {
                NetworkTransport.Init();
            }

            Debug.Log("Server starting...");

            ConnectionConfig config = new ConnectionConfig();
            reliableChannel = config.AddChannel(QosType.Reliable);
            unreliableChannel = config.AddChannel(QosType.Unreliable);

            HostTopology topology = new HostTopology(config, maxConnections);

            hostID = NetworkTransport.AddHost(topology, port);

            if (hostID != -1)
            {
                serverRunning = true;
                Debug.Log("Server started successfuly");
                Debug.Log("Waiting for connections...");
                StartListening();
            }
        }

        public void StartListening()
        {
            if (serverRunning)
            {
                listening = true;
            }
        }

        private void Update()
        {
            if (!serverRunning)
            {
                return;
            }
            else if (listening)
            {
                Listen();
            }
        }

        private void Listen()
        {
            int recHostId;
            int connectionId;
            int channelId;

            byte[] recBuffer = new byte[CaseNet_Config.MAX_BYTE_SIZE];
            int dataSize;
            byte error;

            NetworkEventType type = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer,
              CaseNet_Config.MAX_BYTE_SIZE, out dataSize, out error);

            switch (type)
            {
                case NetworkEventType.Nothing:
                    break;

                case NetworkEventType.ConnectEvent:
                    // Debug.Log("TEAM CONNECTED:" + connectionId + " :D");
                    connections.Add(new CaseConnection(recHostId, connectionId, true));
                    
                    if (clientConnectEvent != null)
                    {
                        clientConnectEvent(connectionId);
                    }
                    else
                    {
                        Debug.Log("clientConnectEvent is empty");
                    }
                    break;

                case NetworkEventType.DisconnectEvent:
                    Debug.Log("TEAM DISCONNECTED:" + connectionId + " :(");
                    //connections.Remove(connectionLookUp(connectionId));
                    clientIds.Remove(connectionId);
                    if (clientDisconnectionEvent != null)
                    {
                        clientDisconnectionEvent(connectionId);
                    }
                    else
                    {
                        Debug.Log("Client disconnect event is empty");
                    }
                    break;

                case NetworkEventType.DataEvent:
                    Debug.Log("Team " + connectionId + " Sent me this shit ");
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

                    break;
            }
        }

        public void ShutDown()
        {
            serverRunning = false;
            NetworkTransport.Shutdown();
        }

        public void SendMessageToClient(int connectionId, int channelId, CaseNetMessage msg)
        {
            byte error;
            byte[] buffer = new byte[CaseNet_Config.MAX_BYTE_SIZE];

            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(buffer);
            formatter.Serialize(stream, msg);

            NetworkTransport.Send(hostID, connectionId, reliableChannel, buffer, CaseNet_Config.MAX_BYTE_SIZE, out error);
        }

        public void SendMessageToAllClients(List<int> connectionsList, int channelId, CaseNetMessage msg)
        {
            byte error;
            byte[] buffer = new byte[CaseNet_Config.MAX_BYTE_SIZE];

            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(buffer);
            formatter.Serialize(stream, msg);

            for (int i = 0; i < connectionsList.Count; i++)
            {
                NetworkTransport.Send(hostID, connectionsList[i], reliableChannel, buffer, CaseNet_Config.MAX_BYTE_SIZE, out error);
            }
        }

        public virtual void OnDataReceivedDefault()
        {
            Debug.Log("Received trash :dd");
        }

        public void StopServer()
        {
            NetworkTransport.RemoveHost(hostID);
            serverRunning = false;
            Debug.Log("Server Stopped...");

            //unsubscribe everything from the events
            clientConnectEvent = null;
            clientDisconnectionEvent = null;
            OnDataEvent = null;
        }

        public bool DisconnectClient(int connId)
        {
            if (!HasClient(connId))
            {
                Debug.Log("DisconnectClient( " + connId + " ) Failed with reason 'Client with id does not exist!'");
                return false;
            }

            byte error;
            NetworkTransport.Disconnect(hostID, connId, out error);

            if (NetUtils.IsNetworkError(error))
            {
                Debug.Log("DisconnectClient( " + connId + " ) Failed with reason '" + NetUtils.GetNetworkError(error) + "'.");
                return false;
            }

            return true;

        }

        public bool HasClient(int id)
        {
             return clientIds.Exists(cId => cId == id);
        }

        //public IEnumerator ServerLoop()
        //{
        //    int recHostId;
        //    int connectionId;
        //    int channelId;

        //    byte[] recBuffer = new byte[CaseNet_Config.MAX_BYTE_SIZE];
        //    int dataSize;
        //    byte error;

        //    NetworkEventType type = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer,
        //      CaseNet_Config.MAX_BYTE_SIZE, out dataSize, out error);

        //    switch (type)
        //    {
        //        case NetworkEventType.Nothing:
        //            break;

        //        case NetworkEventType.ConnectEvent:
        //            Debug.Log("TEAM CONNECTED:" + connectionId + " :D");
        //            if (clientConnectEvent != null)
        //            {
        //                clientConnectEvent(connectionId);
        //            }
        //            else
        //            {
        //                Debug.Log("clientConnectEvent is empty");
        //            }
        //            break;

        //        case NetworkEventType.DisconnectEvent:
        //            Debug.Log("TEAM DISCONNECTED:" + connectionId + " :(");
        //            if (clientDisconnectionEvent != null)
        //            {
        //                clientDisconnectionEvent(connectionId);
        //            }
        //            else
        //            {
        //                Debug.Log("Client disconnect event is empty");
        //            }
        //            break;

        //        case NetworkEventType.DataEvent:
        //            Debug.Log("Team " + connectionId + " Sent me this shit ");
        //            BinaryFormatter formatter = new BinaryFormatter();
        //            MemoryStream stream = new MemoryStream(recBuffer);
        //            CaseNetMessage msg = (CaseNetMessage)formatter.Deserialize(stream);
        //            if (OnDataEvent != null)
        //            {
        //                OnDataEvent(recHostId, connectionId, channelId, msg);
        //            }
        //            else
        //            {
        //                Debug.Log("OnDataEvent  is empty");
        //            }
        //            break;

        //        default:
        //        case NetworkEventType.BroadcastEvent:
        //            Debug.Log("Default broadcast");
        //            break;
        //    }

        //    yield return new WaitForEndOfFrame();
        //}
        public CaseConnection connectionLookUp(int id)
        {
            return connections.First(c => c.connConnectionId == id);
        }
    }
}