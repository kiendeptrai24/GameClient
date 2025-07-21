using UnityEngine;
using System;
using SocketIOClient;
using System.Collections.Generic;
using Newtonsoft.Json;

public class NetworkManager : Singleton<NetworkManager>
{
    public bool IsConnected { get; private set; } = false;
    public static SocketIOUnity socket;
    [SerializeField] private bool isConnect = false;
    [SerializeField] private string apiServer = "https://perfectly-kind-toucan.ngrok-free.app";
    [SerializeField] private ThirdPersonController thirdPersonController = null;
    [Header("User Manager")]
    public User Owner;
    public bool IsOwner => Owner != null;

    public Dictionary<string,User> users = new Dictionary<string, User>();
    public List<User> listUser = new List<User>();

    protected override void Awake()
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
            IsConnected = true;
        };
        
        socket.OnDisconnected += (sender, e) =>
        {
            Debug.Log("Disconnected from server");
        };
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
        socket.On("transform_delta", response =>
        {
            Debug.Log("transform_delta");
            string json = response.GetValue<string>();
            TransformDeltaPacket packet = JsonConvert.DeserializeObject<TransformDeltaPacket>(json);

            // Ví dụ: dùng vị trí
            Vector3 position = new Vector3(
                packet.Delta.Position.X,
                packet.Delta.Position.Y,
                packet.Delta.Position.Z
            );

            // Áp dụng cho object nào đó:
            thirdPersonController?.setpos( position );
        });

        socket.Connect();
    }
    public void getposition(Vector3 vector3)
    {

        var transformDelta = new TransformDeltaPacket
        {
            Id = "player-id", // Nếu có ID thì gán, nếu không thì để null hoặc bỏ
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Delta = new DeltaData
            {
                Position = new Vector3Data
                {
                    X = vector3.x,
                    Y = 0f,        // Gán mặc định nếu chưa dùng
                    Z = vector3.z
                },
                Rotation = new Vector3Data(), // Nếu không dùng, vẫn phải khởi tạo
                Velocity = new Vector3Data()  // Tương tự
            }
        };

        string json = JsonConvert.SerializeObject(transformDelta);
        socket.Emit("player_input", json);
    }
}
public class TransformDeltaPacket
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("delta")]
    public DeltaData Delta { get; set; }

    [JsonProperty("timestamp")]
    public long Timestamp { get; set; }
}

public class DeltaData
{
    [JsonProperty("position")]
    public Vector3Data Position { get; set; }

    [JsonProperty("rotation")]
    public Vector3Data Rotation { get; set; }

    [JsonProperty("velocity")]
    public Vector3Data Velocity { get; set; }
}

public class Vector3Data
{
    [JsonProperty("x")]
    public float X { get; set; }

    [JsonProperty("y")]
    public float Y { get; set; }

    [JsonProperty("z")]
    public float Z { get; set; }
}
