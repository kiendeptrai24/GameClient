using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;

public class ChatManager_UI : Singleton<ChatManager_UI>
{
    [SerializeField] private Button OnChatBtn;

    [Header("Messege")]
    [SerializeField] private RectTransform messegeContent;
    [SerializeField] private MessegeUI messegePrefab;
    protected override void Awake()
    {
        base.Awake();
        LoadComponent();
    }
    protected override void Start()
    {
        base.Start();
        OnChatBtn.onClick.AddListener(() => { ShowCreateChatPopup(); });
    }
    private void ShowCreateChatPopup()
    {
        var popup = PopupManager.Instance.GetPopup<CreateChatPopup>();
        var data = new BasePopupData("CHAT ROOM")
        {
            ValidCharacters = CharacterGroups.AllCharacters,
            CharacterLimit = 50
        };
        popup?.ShowPopup(data,
            onConfirm: (CreateChatData result) =>
            {
                ChatManager.Instance.ChatMessage(result.message);
                popup.SetChatMessage(string.Empty);
            },
            onCancel: () =>
            {
                Debug.Log("Create room cancelled");
                popup.SetChatMessage(string.Empty);
            }
        );
    }
    public void ShowMessege(Messege message)
    {
        MessegeUI newUser = Instantiate(messegePrefab, messegeContent);
        newUser.SetData(message);
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        OnChatBtn = GetComponentsInChildren<Button>().FirstOrDefault(btn => btn.name == "OnChatBtn");
        messegeContent = GetComponentsInChildren<RectTransform>().FirstOrDefault(txt => txt.gameObject.name == "messegeContent");
    }
}
