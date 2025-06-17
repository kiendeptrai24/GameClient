using UnityEngine;
using SocketIOClient;
using System.Text.Json;
using System.Net.Sockets;

public class LobbyManager : Singleton<LobbyManager>
{
    SocketIOUnity socket;
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
            Debug.Log("👤 Player joined: " + response.GetValue<string>());
        });
        socket.On("room_exists", response =>
        {
            Debug.Log("👤 Room: " + response.GetValue<string>());
        });
        socket.On("player_left", response =>
        {
            Debug.Log("👤 Room " + response.GetValue<string>());
        });
        socket.On("room_destroyed", response =>
        {
            Debug.Log("👤 Room: " + response.GetValue<string>());
        });

        // Gửi yêu cầu tạo phòng

        // Nếu muốn tham gia phòng thì dùng:
        // socket.Emit("join_room", roomId);
    }
    public void CreateLobby(string roomId) => socket.Emit("create_room", roomId);
    public void JoinLobby(string roomId) => socket.Emit("join_room", roomId);
    public void LeaveLobby(string roomId) => socket.Emit("leave_room", roomId);
    public void DestroyRoom(string roomId) => socket.Emit("destroy_room", roomId);
    public void KickPlayerLobby(string roomId) => socket.Emit("kickplayer_room", roomId);

}
