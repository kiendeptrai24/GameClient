using UnityEngine;
using System;
using SocketIOClient;

public class NetworkManager : MonoBehaviour
{
    public static SocketIOUnity socket;

    private void Awake()
    {
        if (socket == null)
        {
            OnConnectToServer();
        }
        DontDestroyOnLoad(gameObject);
    }
    [ContextMenu("Connect to server")]
    public void OnConnectToServer()
    {
        var uri = new Uri("http://localhost:3020");
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("✅ Connected to server");
        };

        socket.OnDisconnected += (sender, e) =>
        {
            Debug.Log("🔌 Disconnected from server");
        };

        socket.Connect();
    }
}