using System;
using TMPro;
using UnityEngine;
using System.Linq;
[Serializable]
public class CreateChatData
{
    public string userId;
    public string roomId;
    public string message;
    public string timestamp;
    public CreateChatData(string userId, string roomId, string message, string timestamp)
    {
        this.userId = userId;
        this.roomId = roomId;
        this.message = message;
        this.timestamp = timestamp;
    }
}
public class CreateChatPopup : BasePopup<BasePopupData, CreateChatData>
{
    [SerializeField] private TMP_InputField messageTxt;
    public override void Hide()
    {
        base.Hide();
    }

    public override void Show()
    {
        base.Show();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override CreateChatData GetResult()
    {
        string message = messageTxt?.text ?? "";
        return new CreateChatData("","", message, "");
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        messageTxt = GetComponentsInChildren<TMP_InputField>().FirstOrDefault(x => x.name == "messageTxt");
        okBtn = GetComponentsInChildren<Button_UI>().FirstOrDefault(x => x.name == "okBtn");
        closeBtn = GetComponentsInChildren<Button_UI>().FirstOrDefault(x => x.name == "closeBtn");
    }
    protected override void SetupPopupData(BasePopupData data)
    {
        base.SetupPopupData(data);
        if (messageTxt != null)
        {
            messageTxt.characterLimit = data.CharacterLimit;
            messageTxt.text = "";

            if (!string.IsNullOrEmpty(data.ValidCharacters))
            {
                messageTxt.onValidateInput = (string text, int charIndex, char addedChar) =>
                    ValidateChar(data.ValidCharacters, addedChar);
            }

            messageTxt.Select();
        }
    }


    protected override void Start()
    {
        base.Start();
    }

    protected override bool ValidateResult(CreateChatData result)
    {
        if (string.IsNullOrEmpty(result.message))
        {
            Debug.LogWarning("Message cannot be empty!");
            return false;
        }
        return true;
    }
}
