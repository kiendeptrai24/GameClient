using UnityEngine;
using System;
using SocketIOClient;
using System.Collections.Generic;
using Newtonsoft.Json;

public class NetworkManager : MonoBehaviour
{

    public static SocketIOUnity socket;
    [SerializeField] private bool isConnect = false;
    [SerializeField] private string apiServer = "https://perfectly-kind-toucan.ngrok-free.app";

    [Header("User Manager")]
    public User Owner;
    public bool IsOwner => Owner != null;

    public Dictionary<string,User> users = new Dictionary<string, User>();
    public List<User> listUser = new List<User>();

    private void Awake()
    {
        if (socket == null && isConnect)
        {
            OnConnectToServer();
        }
        DontDestroyOnLoad(gameObject);
    }
    [ContextMenu("Connect to server")]
    public void OnConnectToServer()
    {
        Debug.Log("contectting to server ...");
        var uri = new Uri(apiServer);
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("Connected to server");
        };

        socket.OnDisconnected += (sender, e) =>
        {
            Debug.Log("Disconnected from server");
        };
        socket.On("another_user_login", response =>
        {
           
            Debug.Log("Another user login succsecful: " + response.GetValue<string>());
            string json = response.GetValue<string>();
            User user = JsonConvert.DeserializeObject<User>(json);
            if (!users.ContainsKey(user.id))
            {
                users.Add(user.id, user);
                listUser.Add(user);
            }
        });
        socket.On("user_login", response =>
        {
            string json = response.GetValue<string>();
            User user = JsonConvert.DeserializeObject<User>(json);
            Debug.Log("User login succsecful: " + user.name);

            Owner = user;
            users.Add(user.id, user);
            listUser.Add(user);
            
        });

        socket.Connect();
    }
}