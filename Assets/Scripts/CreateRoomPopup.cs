using System;
using UnityEngine;
using TMPro;
using System.Linq;

[System.Serializable]
public class CreateRoomData
{
    public string roomId;
    public int maxMember;
    public string mode;
    public CreateRoomData(string roomId, string mode,int maxmember) 
    { 
        this.roomId = roomId;
        this.mode = mode;   
        this.maxMember = maxmember;
    }
}
[System.Serializable]
public class BasePopupData : IPopupData
{
    public string Title { get; set; }
    public string ValidCharacters { get; set; } = "";
    public int CharacterLimit { get; set; } = 50;

    public BasePopupData(string title)
    {
        Title = title;
    }
}

public class CreateRoomPopup : BasePopup<BasePopupData, CreateRoomData>
{
    [SerializeField] private TMP_InputField roomIdInput;
    [SerializeField] private TMP_Dropdown maxMemberDropdown;
    [SerializeField] private TMP_Dropdown modeDropdown;

    protected override void SetupPopupData(BasePopupData data)
    {
        base.SetupPopupData(data);

        if (roomIdInput != null)
        {
            roomIdInput.characterLimit = data.CharacterLimit;
            roomIdInput.text = "";

            if (!string.IsNullOrEmpty(data.ValidCharacters))
            {
                roomIdInput.onValidateInput = (string text, int charIndex, char addedChar) =>
                    ValidateChar(data.ValidCharacters, addedChar);
            }

            roomIdInput.Select();
        }
    }

    protected override CreateRoomData GetResult()
    {
        string roomId = roomIdInput?.text ?? "";
        string mode = modeDropdown?.options[modeDropdown.value].text ?? "";
        int maxMember = (maxMemberDropdown?.value ?? 0) + 1;

        return new CreateRoomData(roomId, mode, maxMember);
    }

    protected override bool ValidateResult(CreateRoomData result)
    {
        if (string.IsNullOrEmpty(result.roomId))
        {
            Debug.LogWarning("Room ID cannot be empty!");
            return false;
        }
        return true;
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        roomIdInput = GetComponentsInChildren<TMP_InputField>().FirstOrDefault(x => x.name == "roomId");
        maxMemberDropdown = GetComponentsInChildren<TMP_Dropdown>().FirstOrDefault(x => x.name == "maxMember");
        modeDropdown = GetComponentsInChildren<TMP_Dropdown>().FirstOrDefault(x => x.name == "mode");
    }

    public override void Show()
    {
        PopupAnimation.ShowPopup(rect, group, 0.5f);
    }

    public override void Hide()
    {
        PopupAnimation.HidePopup(rect, group, 0.5f);
    }
}
