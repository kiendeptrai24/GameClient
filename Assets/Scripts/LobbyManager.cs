using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using SocketIOClient;
using System.Threading.Tasks;
using System;
using static UnityEditor.Progress;
using UnityEditor;

[System.Serializable]
public class User
{
    public string id;
    public string name;
    public string room;
}

public class LobbyManager : Singleton<LobbyManager>
{

    SocketIOUnity socket;
    private List<User> userList;
    private List<Room> rooms;
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
        socket.On("chat_messege", response =>
        {

        });
         
        //socket.On("room_list", response =>
        //{
        //    rooms.Clear();
        //    var jsonString = response.GetValue<string>();
        //    Debug.Log(jsonString);
        //    rooms = JsonConvert.DeserializeObject<List<Room>>(jsonString);
        //    // In thử
        //    foreach (var kvp in rooms)
        //    {

        //        foreach (var user in kvp.members)
        //        {
        //            Debug.Log($"👤 User ID: {user.id}, Name: {user.name}");
        //        }
        //    }
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
    public async Task<Messege> WaitForUserChatAsync(string chatMessege)
    {
        var tcs = new TaskCompletionSource<Messege>();

        Action<SocketIOResponse> handler = null;
        handler = (response) =>
        {
            var json = response.GetValue<string>();
            var messege = JsonConvert.DeserializeObject<Messege>(json);

            // Gỡ handler để tránh gọi lại
            socket.Off("chat_messege");

            // Hoàn tất Task
            tcs.TrySetResult(messege);
        };

        socket.On("chat_messege", handler);

        // Gửi yêu cầu
        socket.Emit("chat_messege", chatMessege);
        Debug.Log($"Update Thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
        // Đợi kết quả (non-blocking)
        return await tcs.Task;
    }
    public async Task<List<Room>> WaitForRoomListAsync()
    {
        var tcs = new TaskCompletionSource<List<Room>>();

        Action<SocketIOResponse> handler = null;
        handler = (response) =>
        {

            var jsonString = response.GetValue<string>();
            Debug.Log(jsonString);
            List<Room> rooms = JsonConvert.DeserializeObject<List<Room>>(jsonString);
            // In thử
            foreach (var kvp in rooms)
            {

                foreach (var user in kvp.members)
                {
                    Debug.Log($"👤 User ID: {user.id}, Name: {user.name}");
                }
            }

            // Gỡ handler để tránh gọi lại
            socket.Off("room_list");

            // Hoàn tất Task
            tcs.TrySetResult(rooms);
        };

        socket.On("room_list", handler);

        // Gửi yêu cầu
        socket.Emit("checklist_room");
        Debug.Log($"Update Thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
        // Đợi kết quả (non-blocking)
        return await tcs.Task;
    }

    public void CreateLobby(string roomId) => socket.Emit("create_room", roomId);
    public void JoinLobby(string roomId) => socket.Emit("join_room", roomId);
    public void LeaveLobby(string roomId) => socket.Emit("leave_room", roomId);
    public void DestroyRoom(string roomId) => socket.Emit("destroy_room", roomId);
    public void KickPlayerLobby(string roomId) => socket.Emit("kickplayer_room", roomId);
    public void GetUsersOnLobby(string roomId) => socket.Emit("getuser_room", roomId);
    public void ChatOnLobby(string messege) => socket.Emit("chat_messege", messege);

    public List<User> GetUserOnRooms() => userList;    
    public List<Room> GetAllRooms() => rooms;
}
