using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Newtonsoft.Json;

public class LobbyUI : KienBehaviour
{
    [Header("Button Handler")]
    [SerializeField] private Button OnCreateRoomBtn;
    [SerializeField] private Button OnJoinRoomBtn;
    [SerializeField] private Button OnLeaveRoomBtn;

    [SerializeField] private Button OnReloadRoomBtn;
    [SerializeField] private Button OnReloadUserBtn;
    [SerializeField] private Button OnSendMessege;

    [Space]
    [SerializeField] private TMP_InputField input;
    [SerializeField] private TMP_InputField message;
    [SerializeField] private TMP_Dropdown maxMember;
    [SerializeField] private TMP_Dropdown mode;
    [Space]

    [Header("Messege")]
    [SerializeField] private RectTransform userContent;
    [SerializeField] private UserOnRoomUI userPrefab;

    [Header("Messege")]
    [SerializeField] private RectTransform roomContent;
    [SerializeField] private RoomUI roomPrefab;

    [Header("Messege")]
    [SerializeField] private RectTransform messegeContent;
    [SerializeField] private MessegeUI messegePrefab;

    protected override void Start()
    {
        base.Start();
        OnCreateRoomBtn.onClick.AddListener(() =>
        {
            RoomRequestDTO roomRequest = new RoomRequestDTO(input.text,mode.name,maxMember.value + 1);
            string jsonrequest = JsonConvert.SerializeObject(roomRequest);
            LobbyManager.Instance.CreateLobby(jsonrequest);
        });
        OnJoinRoomBtn.onClick.AddListener(() =>
        {
            LobbyManager.Instance.JoinLobby(input.text);
            LobbyManager.Instance.RefreshRoomList();
        });
        OnLeaveRoomBtn.onClick.AddListener(() => LobbyManager.Instance.LeaveLobby(input.text));
        OnReloadRoomBtn.onClick.AddListener(() =>
        {
            LobbyManager.Instance.RefreshRoomList();
        });
        OnReloadUserBtn.onClick.AddListener(async () =>
        {
            ShowListUserOnRoom(await LobbyManager.Instance.WaitForUserListAsync(input.text));
        });
        OnSendMessege.onClick.AddListener(async () =>
        {
            ShowMessege(await LobbyManager.Instance.WaitForUserChatAsync(message.text));
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
    public void ShowListRoom(List<Room> rooms)
    {
        foreach (Transform child in roomContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var room in rooms)
        {
            RoomUI newUser = Instantiate(roomPrefab, roomContent);
            newUser.SetData(room);
        }
    }
    public void ShowMessege(Messege message)
    {
        Debug.Log("messege");
        MessegeUI newUser = Instantiate(messegePrefab, messegeContent);
        newUser.SetData(message);   
    }

    protected override void LoadComponent()
    {
        OnCreateRoomBtn = GameObject.Find("Create").GetComponent<Button>();
        OnJoinRoomBtn = GameObject.Find("Join").GetComponent<Button>();
        OnLeaveRoomBtn = GameObject.Find("Leave").GetComponent<Button>();

        OnSendMessege = GameObject.Find("SendMessege").GetComponent<Button>();

        OnReloadRoomBtn = GameObject.Find("ReloadRoom").GetComponent<Button>();
        OnReloadUserBtn = GameObject.Find("ReloadUser").GetComponent<Button>();

        input = GameObject.Find("input").GetComponent<TMP_InputField>();
        message = GameObject.Find("inputMessege").GetComponent<TMP_InputField>();

        userContent = GameObject.Find("UserContent").GetComponent<RectTransform>();
        roomContent = GameObject.Find("RoomContent").GetComponent<RectTransform>();
        messegeContent = GameObject.Find("MessegeContent").GetComponent<RectTransform>();

        //OnReloadUserBtn = GameObject.Find("ReloadUser").GetComponent<UserOnRoomUI>();
        //OnReloadUserBtn = GameObject.Find("ReloadUser").GetComponent<RoomUI>();

    }
}
