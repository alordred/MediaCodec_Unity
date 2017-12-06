using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Transport : MonoBehaviour
{

    int myReliableChannelID;
    int socketId;
    int socketPort = 8888;
    int connectionId;

    // Use this for initialization
    void Start()
    {
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();
        myReliableChannelID = config.AddChannel(QosType.Reliable);
        HostTopology topology = new HostTopology(config, 10);
        socketId = NetworkTransport.AddHost(topology, socketPort);
        Debug.Log("Socket Open. SocketId is: " + socketId);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Connect()
    {
        byte error;
        connectionId = NetworkTransport.Connect(socketId, "192.168.0.105", socketPort, 0, out error);
        Debug.Log("Connected to server. ConnectionId: " + connectionId);
    }

    public void SendSocketMessage()
    {
        byte error;
        byte[] buffer = new byte[1024];
    }
}
