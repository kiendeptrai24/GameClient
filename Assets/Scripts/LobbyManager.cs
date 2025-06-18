using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
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
        socket.On("room_getuser", response =>
        {
            var jsonString = response.GetValue<string>();
            Debug.Log(jsonString.ToString());
            userList = JsonConvert.DeserializeObject<List<User>>(jsonString);
            lobbyUI.ShowListUserOnRoom(userList);

        });

    }
    public void CreateLobby(string roomId) => socket.Emit("create_room", roomId);
    public void JoinLobby(string roomId) => socket.Emit("join_room", roomId);
    public void LeaveLobby(string roomId) => socket.Emit("leave_room", roomId);
    public void DestroyRoom(string roomId) => socket.Emit("destroy_room", roomId);
    public void KickPlayerLobby(string roomId) => socket.Emit("kickplayer_room", roomId);
    public void GetUsersOnLobby(string roomId) => socket.Emit("getuser_room", roomId);
    public List<User> GetUserOnRoom() => userList;

}
