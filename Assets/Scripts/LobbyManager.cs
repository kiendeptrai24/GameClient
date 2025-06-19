using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using SocketIOClient;
using System.Threading.Tasks;
using System;
using static UnityEditor.Progress;

[System.Serializable]
public class User
{
    public string id = "";
    public string name = "";
}

public class LobbyManager : Singleton<LobbyManager>
{

    SocketIOUnity socket;
    private List<User> userList;
    private LobbyUI lobbyUI;
    private void Awake()
    {
        lobbyUI = GetComponent<LobbyUI>();
    }
    void Start()
    {

        if (NetworkManager.socket == null)
        {
            Debug.LogError("❌ Socket not initialized");
            return;
        }

        socket = NetworkManager.socket;

        socket.On("room_created", response =>
        {
            Debug.Log("🏠 Room created: " + response.GetValue<string>());
        });

        socket.On("player_joined", response =>
        {
            Debug.Log("➕ Player joined: " + response.GetValue<string>());
        });

        socket.On("room_exists", response =>
        {
            Debug.Log("ℹ️ Room exists: " + response.GetValue<string>());
        });

        socket.On("player_left", response =>
        {
            Debug.Log("➖ Player left: " + response.GetValue<string>());
        });

        socket.On("room_destroyed", response =>
        {
            Debug.Log("💥 Room destroyed: " + response.GetValue<string>());
        });

        socket.On("room_not_found", response =>
        {
            Debug.Log("❌ Room not found: " + response.GetValue<string>());
        });
        socket.On("room_list", response =>
        {

            var jsonString = response.GetValue<string>();
            Debug.Log(jsonString);
            Dictionary<string, List<User>> rooms = JsonConvert.DeserializeObject<Dictionary<string, List<User>>>(jsonString);
            // In thử
            foreach (var kvp in rooms)
            {
                Debug.Log($"🔑 Room: {kvp.Key}");
                foreach (var user in kvp.Value)
                {
                    Debug.Log($"👤 User ID: {user.id}, Name: {user.name}");
                }
            }
        });
        //socket.On("room_getuser", response =>
        //{
        //    var jsonString = response.GetValue<string>();
        //    Debug.Log(jsonString.ToString());
        //    userList = JsonConvert.DeserializeObject<List<User>>(jsonString);
        //    Debug.Log($"Update Thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
        //    lobbyUI.UpdateUser(userList);
        //});

    }
    public async Task<List<User>> WaitForUserListAsync(string roomId)
    {
        var tcs = new TaskCompletionSource<List<User>>();

        Action<SocketIOResponse> handler = null;
        handler = (response) =>
        {
            var json = response.GetValue<string>();
            var users = JsonConvert.DeserializeObject<List<User>>(json);

            // Gỡ handler để tránh gọi lại
            socket.Off("room_getuser");

            // Hoàn tất Task
            tcs.TrySetResult(users);
        };

        socket.On("room_getuser", handler);

        // Gửi yêu cầu
        socket.Emit("getuser_room", roomId);
        Debug.Log($"Update Thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
        // Đợi kết quả (non-blocking)
        return await tcs.Task;
    }
    public async Task<Dictionary<string, List<User>>> WaitForRoomListAsync()
    {
        var tcs = new TaskCompletionSource<Dictionary<string, List<User>>>();

        Action<SocketIOResponse> handler = null;
        handler = (response) =>
        {
            var jsonString = response.GetValue<string>();
            Debug.Log(jsonString);
            Dictionary<string, List<User>> rooms = JsonConvert.DeserializeObject<Dictionary<string, List<User>>>(jsonString);

            // Gỡ handler để tránh gọi lại
            socket.Off("checklist_room");

            // Hoàn tất Task
            tcs.TrySetResult(rooms);
        };

        socket.On("checklist_room", handler);

        // Gửi yêu cầu
        socket.Emit("room_list");
        Debug.Log($"Update Thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
        // Đợi kết quả (non-blocking)
        return await tcs.Task;
    }
    public List<User> GetUserOnRoom()
    {
        Debug.Log(userList.ToString());
        return userList;    
    }
    public void CreateLobby(string roomId) => socket.Emit("create_room", roomId);
    public void JoinLobby(string roomId) => socket.Emit("join_room", roomId);
    public void LeaveLobby(string roomId) => socket.Emit("leave_room", roomId);
    public void DestroyRoom(string roomId) => socket.Emit("destroy_room", roomId);
    public void KickPlayerLobby(string roomId) => socket.Emit("kickplayer_room", roomId);
    public void GetUsersOnLobby(string roomId) => socket.Emit("getuser_room", roomId);

}
