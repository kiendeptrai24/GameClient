using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts;

public class LobbyUI : MonoBehaviour
{

    [SerializeField] private Button OnCreateRoomBtn;
    [SerializeField] private Button OnJoinRoomBtn;
    [SerializeField] private Button OnLeaveRoomBtn;
    [SerializeField] private TMP_InputField input;
    [SerializeField] private Transform content;
    [SerializeField] private UserOnRoomUI prefab;
    private List<string> userOnRoom = new List<string>();
    private void Start()
    {
        OnCreateRoomBtn.onClick.AddListener(() => LobbyManager.Instance.CreateLobby(input.text));
        OnJoinRoomBtn.onClick.AddListener(() => {
            LobbyManager.Instance.JoinLobby(input.text);
            userOnRoom = LobbyManager.Instance.GetUserOnRoom();
            ShowListUserOnRoom();
        });
        OnLeaveRoomBtn.onClick.AddListener(() => LobbyManager.Instance.LeaveLobby(input.text));
    }

    private void ShowListUserOnRoom()
    {
        foreach (var id in userOnRoom)
        {
            UserOnRoomUI newUser = Instantiate(prefab, content);
            newUser.id.text = id; 
        }
    }
}
