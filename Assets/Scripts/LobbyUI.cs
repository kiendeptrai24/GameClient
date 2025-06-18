using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts;
using NUnit.Framework.Constraints;

public class LobbyUI : MonoBehaviour
{

    [SerializeField] private Button OnCreateRoomBtn;
    [SerializeField] private Button OnJoinRoomBtn;
    [SerializeField] private Button OnLeaveRoomBtn;
    [SerializeField] private Button OnReloadRoomBtn;
    [SerializeField] private TMP_InputField input;
    [SerializeField] private RectTransform content;
    [SerializeField] private UserOnRoomUI prefab;
    private List<UserOnRoomUI> userOnRoom = new();
    private void Start()
    {
        OnCreateRoomBtn.onClick.AddListener(() =>
        {
            LobbyManager.Instance.CreateLobby(input.text);
            LobbyManager.Instance.GetUsersOnLobby(input.text);
        });
        OnJoinRoomBtn.onClick.AddListener(() =>
        {
            LobbyManager.Instance.JoinLobby(input.text);
            LobbyManager.Instance.GetUsersOnLobby(input.text);
        });
        OnLeaveRoomBtn.onClick.AddListener(() => LobbyManager.Instance.LeaveLobby(input.text));
        OnReloadRoomBtn.onClick.AddListener(() =>
        {
            LobbyManager.Instance.GetUsersOnLobby(input.text);
        });
    }

    public void ShowListUserOnRoom(List<User> users)
    {
        foreach (User user in users)
        {
            UserOnRoomUI newUser = Instantiate(prefab);
            newUser.id.text = $"userId: {user.id} name: {user.name}";
            userOnRoom.Add(newUser);
        }
    }
}
