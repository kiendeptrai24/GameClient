using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class LobbyUI : MonoBehaviour
{
    [Header("Button Handler")]
    [SerializeField] private Button OnCreateRoomBtn;
    [SerializeField] private Button OnJoinRoomBtn;
    [SerializeField] private Button OnLeaveRoomBtn;
    [SerializeField] private Button OnReloadRoomBtn;
    [Space]
    [SerializeField] private TMP_InputField input;
    [Space]
    [SerializeField] private RectTransform userContent;
    [SerializeField] private UserOnRoomUI userPrefab;
    [SerializeField] private RectTransform roomContent;
    [SerializeField] private RoomUI roomPrefab;

    private void Start()
    {
        OnCreateRoomBtn.onClick.AddListener(async () =>
        {
            LobbyManager.Instance.CreateLobby(input.text);
            ShowListUserOnRoom(await LobbyManager.Instance.WaitForUserListAsync(input.text));
            Debug.Log($"Update Thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
        });
        OnJoinRoomBtn.onClick.AddListener(async () =>
        {
            LobbyManager.Instance.JoinLobby(input.text);
            ShowListUserOnRoom(await LobbyManager.Instance.WaitForUserListAsync(input.text));
            Debug.Log($"Update Thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
        });
        OnLeaveRoomBtn.onClick.AddListener(() => LobbyManager.Instance.LeaveLobby(input.text));
        OnReloadRoomBtn.onClick.AddListener(async () =>
        {
            Debug.Log($"Update Thread: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            ShowListUserOnRoom(await LobbyManager.Instance.WaitForUserListAsync(input.text));
        });
    }
    public void ShowListUserOnRoom(List<User> users)
    {
        foreach (Transform child in userContent)
        {
            Destroy(child.gameObject);
        }

        foreach (User user in users)
        {
            UserOnRoomUI newUser = Instantiate(userPrefab, userContent);
            newUser.id.text = $"userId: {user.id} name: {user.name}";
        }
    }
    public void ShowListRoom(Dictionary<string, List<User>> rooms)
    {
        foreach (Transform child in roomContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var user in rooms)
        {
            RoomUI newUser = Instantiate(roomPrefab, roomContent);
            
        }
    }
}
