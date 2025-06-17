using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{

    [SerializeField] private Button OnCreateRoomBtn;
    [SerializeField] private Button OnJoinRoomBtn;
    [SerializeField] private Button OnLeaveRoomBtn;
    [SerializeField] private TMP_InputField input;
    private void Start()
    {
        OnCreateRoomBtn.onClick.AddListener(() => LobbyManager.Instance.CreateLobby(input.text));
        OnJoinRoomBtn.onClick.AddListener(() => LobbyManager.Instance.JoinLobby(input.text));
        OnLeaveRoomBtn.onClick.AddListener(() => LobbyManager.Instance.LeaveLobby(input.text));
    }

}
