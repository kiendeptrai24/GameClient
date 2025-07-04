using UnityEngine;
using System;
using SocketIOClient;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour
{
    public static SocketIOUnity socket;
    public Dictionary<string, User> User = new Dictionary<string, User>();
    public User Owner;
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
        Debug.Log("contectting to server ...");
        var uri = new Uri("https://perfectly-kind-toucan.ngrok-free.app");
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
        socket.On("user_login", response =>
        {
            Debug.Log("🏠 User login succsecful: " + response.GetValue<string>());
            //User user = 
        });

        socket.Connect();
    }
}