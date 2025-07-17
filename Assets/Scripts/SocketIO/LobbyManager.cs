using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using SocketIOClient;
using System.Threading.Tasks;
using System;
using System.Collections;

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
    private LobbySellectedUI lobbyUI;

    [Header("Lobby Info")]
    [SerializeField] private bool autoRefesh;
    [SerializeField] private float refeshRoomListCoolDown = 2;
    private Coroutine refreshCoroutine;

    [SerializeField]
    protected override void Awake()
    {
        lobbyUI = GetComponent<LobbySellectedUI>();
    }
    protected override void Start()
    {
        AddListenerEvent();
    }
    private void AddListenerEvent()
    {
        if (NetworkManager.socket == null)
        {
            Debug.LogError("Socket not initialized");
            return;
        }

        socket = NetworkManager.socket;

        listenerEventSockeIO();

    }

    private void listenerEventSockeIO()
    {
        socket.On("room_created", response =>
        {
            Debug.Log("Room created: " + response.GetValue<string>());
        });

        socket.On("player_joined", response =>
        {
            Debug.Log("Player joined: " + response.GetValue<string>());
        });

        socket.On("room_exists", response =>
        {
            Debug.Log("Room exists: " + response.GetValue<string>());
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
            List<Room> rooms = JsonConvert.DeserializeObject<List<Room>>(jsonString);
            MainThreadDispatcher.RunOnMainThread(() => { lobbyUI.ShowListRoom(rooms); });

        });
        refreshCoroutine = StartCoroutine(AutoRefreshRoomList(refeshRoomListCoolDown));
    }

    IEnumerator AutoRefreshRoomList(float refeshRoomListCoolDown)
    {
        while (autoRefesh)
        {
            RefreshRoomList();
            yield return new WaitForSeconds(refeshRoomListCoolDown);
        }
    }


    public void RefreshRoomList() => socket.Emit("checklist_room");
    
    public async Task<List<User>> WaitForUserListAsync(string roomId)
    {
        var tcs = new TaskCompletionSource<List<User>>();

        Action<SocketIOResponse> handler = null;
        handler = (response) =>
        {
            var json = response.GetValue<string>();
            var users = JsonConvert.DeserializeObject<List<User>>(json);

            socket.Off("room_getuser");
            tcs.TrySetResult(users);
        };

        socket.On("room_getuser", handler);

        socket.Emit("getuser_room", roomId);
        return await tcs.Task;
    }

    public void CreateLobby(string room) => socket.Emit("create_room", room);
    public void JoinLobby(string roomId) => socket.Emit("join_room", roomId);
    public void LeaveLobby(string roomId) => socket.Emit("leave_room", roomId);
    public void DestroyRoom(string roomId) => socket.Emit("destroy_room", roomId);
    public void KickPlayerLobby(string roomId) => socket.Emit("kickplayer_room", roomId);
    public void GetUsersOnLobby(string roomId) => socket.Emit("getuser_room", roomId);
    public void ChatOnLobby(string messege) => socket.Emit("chat_messege", messege);

    public List<User> GetUserOnRooms() => userList;    
    public List<Room> GetAllRooms() => rooms;
}
