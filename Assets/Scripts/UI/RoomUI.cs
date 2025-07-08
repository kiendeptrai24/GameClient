using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    public TextMeshProUGUI id;
    public TextMeshProUGUI host;
    public TextMeshProUGUI maxMembers;
    public TextMeshProUGUI mode;

    public Image sprite;
    public Button joinRoombtn;
    private void Start()
    {
        joinRoombtn = GetComponentInChildren<Button>();
        joinRoombtn.onClick.AddListener(JoinRoom);
    }
    public void SetData(Room room)
    {
        this.id.text = room.id;
        this.host.text = room.host.name;
        this.mode.text = room.mode;
        this.maxMembers.text = room.max.ToString();
    }
    public void JoinRoom() => LobbyManager.Instance.JoinLobby(id.text);
}
[System.Serializable]
public class Room
{
    public string id;
    public string name;
    public User host;
    public List<User> members;
    public string mode;
    public string status;
    public int max;
}
[System.Serializable]
public class RoomRequestDTO
{
    public string roomId;
    public string mode;
    public int max;
    public RoomRequestDTO(string id, string mode, int max)
    {
        this.roomId = id;
        this.mode = mode;
        this.max = max;
    }
}
