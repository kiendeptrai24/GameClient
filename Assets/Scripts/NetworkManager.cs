using UnityEngine;
using System;
using System.Text.Json.Nodes;
using SocketIOClient;
using UnityEngine.UIElements;
using Newtonsoft.Json;
using UnityEngine;
using SocketIOClient;
using System;

public class NetworkManager : MonoBehaviour
{
    public static SocketIOUnity socket;

    private void Awake()
    {
        if (socket == null)
        {
            var uri = new Uri("https://5943-125-235-185-219.ngrok-free.app");
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
            DontDestroyOnLoad(gameObject);
        }
    }
}