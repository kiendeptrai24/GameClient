using UnityEngine;
using System;
using SocketIOClient;
using System.Collections.Generic;
using Newtonsoft.Json;
public class AnimatorPacket { public string id; public string type; public string name; public bool val; }
public class NetworkManager : Singleton<NetworkManager>
{
    public bool IsConnected { get; private set; } = false;
    public static SocketIOUnity socket;
    [SerializeField] private bool isConnect = false;
    [SerializeField] private string apiServer = "https://perfectly-kind-toucan.ngrok-free.app";
    public GameObject player;
    [Header("User Manager")]
    public User Owner;
    public bool IsOwner => Owner != null;

    public Dictionary<string,User> users = new Dictionary<string, User>();
    public List<User> listUser = new List<User>();
    public List<NetworkTransform> clientTransforms = new List<NetworkTransform>();

    public event Action<User> OnSpawn;
    protected override void Awake()
    {
        if (isConnect)
        {
            OnConnectToServer();
        }
        DontDestroyOnLoad(gameObject);
    }
    [ContextMenu("Connect to server")]
    public void OnConnectToServer()
    {
        Connecting();

        socket.OnConnected += (sender, e) =>
        {

            Debug.Log("Connected to server");
            IsConnected = true;

            socket.On("authen_token", response =>
            {
                Debug.Log(response.ToString());
                //UserSession.SetToken(null);
            });
            socket.On("another_user_login", response =>
            {

                Debug.Log("Another user login succsecful: " + response.GetValue<string>());
                string json = response.GetValue<string>();
                User user = JsonConvert.DeserializeObject<User>(json);
                user.isLocal = false;
                if (!users.ContainsKey(user.id))
                {
                    MainThreadDispatcher.RunOnMainThread(() => AddNewPlayer(user));
                }
            });
            socket.On("user_login", response =>
            {
                string json = response.GetValue<string>();
                User user = JsonConvert.DeserializeObject<User>(json);
                user.isLocal = true;
                Owner = user;
                Debug.Log("User login succsecful: " + user.name);
                AddNewPlayer(user);
                socket.Emit("getalluser", "");
            });
            socket.On("getalluser", response =>
            {
                Debug.Log("getalluser");
                string json = response.GetValue().ToString();
                var allUser = JsonConvert.DeserializeObject<List<User>>(json);
                foreach (var user in allUser)
                {
                    if(Owner.id != user.id)
                    {
                        AddNewPlayer(user);
                    }
                }

            });
            socket.On("transform_delta", response =>
            {
                try
                {

                    string json = response.GetValue().ToString();
                    TransformDeltaPacket packet = JsonConvert.DeserializeObject<TransformDeltaPacket>(json);

                    Vector3 pos = new Vector3(packet.delta.position.x, packet.delta.position.y, packet.delta.position.z);
                    Quaternion rot = Quaternion.Euler(packet.delta.rotation.x, packet.delta.rotation.y, packet.delta.rotation.z);
                    MainThreadDispatcher.RunOnMainThread(() =>
                    {
                        if (NetworkObject.AllObjects.TryGetValue(packet.id, out var netObj))
                        {
                            var netTransform = netObj.GetComponent<NetworkTransform>();
                            if (netTransform != null) netTransform.ApplyTransform(pos, rot);
                        }
                    });
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.ToString());
                }
            });
          
            socket.On("anim_update", response =>
            {
                string json = response.GetValue<string>();
                var packet = JsonConvert.DeserializeObject<AnimatorPacket>(json);

                MainThreadDispatcher.RunOnMainThread(() =>
                {
                    if (NetworkObject.AllObjects.TryGetValue(packet.id, out var netObj))
                    {
                        var netAnimator = netObj.GetComponent<NetworkAnimator>();
                        if (netAnimator != null)
                            netAnimator.ApplyAnimUpdate(packet.type, packet.name, packet.val);
                    }
                });
            });
        };

        socket.OnDisconnected += (sender, e) =>
        {
            Debug.Log("Disconnected from server");
        };

        socket.Connect();
    }
    protected override void OnApplicationQuit()
    {
        socket.Emit("disconnect", "");
        socket.Disconnect();
    }
    private void AddNewPlayer(User user)
    {
        users.Add(user.id, user);
        listUser.Add(user);
    }
    private void Connecting()
    {
        Debug.Log("contectting to server ...");
        var uri = new Uri(apiServer);
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
    }
    public void getposition(Vector3 vector3,string clientId)
    {
        var transformDelta = new
        {
            newTransform = new
            {
                position = new Vector3Data { x = vector3.x, y = vector3.y, z = vector3.z },
                rotation = new Vector3Data { x = 0f, y = 0f, z = 0f },
                scale = new Vector3Data { x = 1f, y = 1f, z = 1f },
                velocity = new Vector3Data { x = 1f, y = 0f, z = 0f },
                angularVelocity = new Vector3Data { x = 0f, y = 5f, z = 0f }
            }

        };

        string json = JsonConvert.SerializeObject(transformDelta);
        
        socket.Emit("player_input", json);
    }
}
public class TransformDeltaPacket
{
    public string id { get; set; }
    public DeltaData delta { get; set; }
    public long timestamp { get; set; }
}

public class DeltaData
{
    public Vector3Data position { get; set; }
    public Vector3Data rotation { get; set; }
}

public class Vector3Data
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
}
