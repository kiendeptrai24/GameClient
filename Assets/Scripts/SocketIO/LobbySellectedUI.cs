using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using static Unity.Cinemachine.CinemachineSplineRoll;


public static class CharacterGroups
{
    public const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
    public const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string Digits = "0123456789";
    public const string SpecialCharacters = "@._-!#$%&*()+=<>?/\\|~^";
    public const string LettersAndDigits = LowercaseLetters + UppercaseLetters + Digits;
    public const string AllCharacters = LowercaseLetters + UppercaseLetters + Digits + SpecialCharacters;  
}

public class LobbySellectedUI : Singleton<LobbySellectedUI>
{
    [Header("Visual Weapon")]
    [SerializeField] private Button OnCreateRoomBtn;
    [SerializeField] private Button OnJoinRoomBtn;
    [SerializeField] private Button OnReloadRoomBtn;
    [Space]
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private TextMeshProUGUI roomMaxmember;
    [SerializeField] private TextMeshProUGUI roomMode;

    [Header("Lobby")]
    [SerializeField] private RectTransform roomContent;
    [SerializeField] private RoomUI roomPrefab;
    private List<RoomUI> roomList = new List<RoomUI>();

    private RoomUI currentRoom;
    


    [SerializeField] private Button OnLeaveRoomBtn;
    [SerializeField] private Button OnReloadUserBtn;
    [SerializeField] private Button OnSendMessege;
    
    
    [Space]
    [SerializeField] private RectTransform userContent;
    [SerializeField] private UserOnRoomUI userPrefab;



    [Header("Messege")]

    [Header("Messege")]
    [SerializeField] private RectTransform messegeContent;
    [SerializeField] private MessegeUI messegePrefab;

    protected override void Start()
    {
        base.Start();
        OnCreateRoomBtn.onClick.AddListener(() =>
        {
            ShowCreateRoomPopup();
        });
        OnJoinRoomBtn.onClick.AddListener(() =>
        {
            LobbyManager.Instance.JoinLobby(currentRoom.id.ToString());
            LobbyManager.Instance.RefreshRoomList();
        });
        OnReloadRoomBtn.onClick.AddListener(() => LobbyManager.Instance.RefreshRoomList());

        //{
        //    OnLeaveRoomBtn.onClick.AddListener(() => LobbyManager.Instance.LeaveLobby("text"));
        //    OnReloadUserBtn.onClick.AddListener(async () =>
        //    {
        //        ShowListUserOnRoom(await LobbyManager.Instance.WaitForUserListAsync("text"));
        //    });
        //    OnSendMessege.onClick.AddListener(async () =>
        //    {
        //        ShowMessege(await LobbyManager.Instance.WaitForUserChatAsync("text"));
        //    });

        //}
    }
    private void ShowCreateRoomPopup()
    {
        var popup = PopupManager.Instance.GetPopup<CreateRoomPopup>();
        var data = new BasePopupData("LOBBY ROOM")
        {
            ValidCharacters = CharacterGroups.LettersAndDigits,
            CharacterLimit = 20
        };

        popup?.ShowPopup(data,
            onConfirm: (CreateRoomData result) =>
            {
                Debug.Log($"Room created: {result.roomId}, Mode: {result.mode}, Max: {result.maxMember}");
                RoomRequestDTO roomRequest = new RoomRequestDTO(result.roomId, result.mode, result.maxMember);
                string jsonrequest = JsonConvert.SerializeObject(roomRequest);
                LobbyManager.Instance.CreateLobby(jsonrequest);
                LobbyManager.Instance.RefreshRoomList();
            },
            onCancel: () =>
            {
                Debug.Log("Create room cancelled");
            }
        );
    }

    public void RoomSelected(RoomUI room)
    {
        currentRoom?.HideFocus();
        currentRoom = room;
        if (currentRoom == null) return;
        UpdateRoomInfoUI();
        currentRoom.ShowFocus();

    }
    private void UpdateRoomInfoUI()
    {
        roomName.text = currentRoom.data.name;
        roomMaxmember.text = currentRoom.data.max.ToString();
        roomMode.text = currentRoom.data.mode;
    }
    public void ShowListRoom(List<Room> rooms)
    {
        foreach (Transform child in roomContent)
        {
            Destroy(child.gameObject);
        }

        roomList.Clear();
        currentRoom = null;

        if (roomPrefab == null)
        {
            Debug.LogError("❌ roomPrefab is null! Check if it was destroyed.");
            return;
        }

        foreach (var room in rooms)
        {
            RoomUI newUser = Instantiate(roomPrefab, roomContent);
            newUser.SetData(room);
            roomList.Add(newUser);
        }

        currentRoom = roomList.Count > 0 ? roomList[0] : null;
    }

    protected override void LoadComponent()
    {
        OnCreateRoomBtn = GetComponentsInChildren<Button>().FirstOrDefault(btn => btn.gameObject.name == "OnCreateRoomBtn");
        OnJoinRoomBtn = GetComponentsInChildren<Button>().FirstOrDefault(btn => btn.gameObject.name == "OnJoinRoomBtn");
        OnReloadRoomBtn = GetComponentsInChildren<Button>().FirstOrDefault(btn => btn.gameObject.name == "OnReloadRoomBtn");
        roomContent = GetComponentsInChildren<RectTransform>().FirstOrDefault(btn => btn.gameObject.name == "roomContent");

        roomName = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(txt => txt.gameObject.name == "roomName");
        roomMaxmember = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(txt => txt.gameObject.name == "roomMaxmember");
        roomMode = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(txt => txt.gameObject.name == "roomMode");
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
    public void ShowMessege(Messege message)
    {
        Debug.Log("messege");
        MessegeUI newUser = Instantiate(messegePrefab, messegeContent);
        newUser.SetData(message);
    }
}
