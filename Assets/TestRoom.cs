using UnityEngine;
using UnityEngine.UI;

public class TestRoom : MonoBehaviour
{
    [SerializeField] private Button OnCreateRoom;
    [SerializeField] private Button OnJoinRoom;
    [SerializeField] private Button OnDestroyRoom;
    [SerializeField] private Button OnStartRoom;
    private void Start()
    {
        OnJoinRoom.onClick.AddListener(() => { LobbyManager.Instance.JoinLobby("room01"); });
        OnCreateRoom.onClick.AddListener(() => { LobbyManager.Instance.CreateLobby("{\r\n  \"roomId\": \"room01\",\r\n  \"mode\": \"solo\",\r\n  \"max\": 4\r\n}"); });
        OnStartRoom.onClick.AddListener(() => { LobbyManager.Instance.StartGame(""); });
        OnDestroyRoom.onClick.AddListener(() => { LobbyManager.Instance.DestroyRoom("room01"); });

    }
}
