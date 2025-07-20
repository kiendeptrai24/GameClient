using System.Linq;
using TMPro;
using UnityEngine;
public class CreateSignInData
{
    public string Email { get; set; }
    public string Password { get; set; }

    public CreateSignInData(string email,string password) 
    {
        Email = email;
        Password = password;
    }
    public CreateSignInData() { }
}
public class CreateSignInPopup : BasePopup<BasePopupData, CreateSignInData>
{
    [SerializeField] private TMP_InputField emailTbx { get; set; }
    [SerializeField] private TMP_InputField passwordTbx { get; set; }

    public override void Hide()
    {
        PopupAnimation.HidePopup(rect, group, .5f);
    }

    public override void Show()
    {
        base.Show();
        PopupAnimation.ShowPopup(rect, group, .5f);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }
    protected override CreateSignInData GetResult()
    {
        string email = emailTbx?.text ?? "";
        string password = passwordTbx?.text ?? "";
        return new CreateSignInData(email, password);
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        emailTbx = GetComponentsInChildren<TMP_InputField>().FirstOrDefault(x => x.name == "emailTbx");
        passwordTbx = GetComponentsInChildren<TMP_InputField>().FirstOrDefault(x => x.name == "passwordTbx");
    }

    protected override void OnCancelClicked()
    {
        base.OnCancelClicked();
    }

    protected override void OnOkClicked()
    {
        base.OnOkClicked();
    }

    protected override void SetupPopupData(BasePopupData data)
    {
        base.SetupPopupData(data);
        if (emailTbx != null)
        {
            emailTbx.characterLimit = data.CharacterLimit;
            emailTbx.text = "";

            if (!string.IsNullOrEmpty(data.ValidCharacters))
            {
                emailTbx.onValidateInput = (string text, int charIndex, char addedChar) =>
                    ValidateChar(data.ValidCharacters, addedChar);
            }

            emailTbx.Select();
        }
        if (passwordTbx != null)
        {
            passwordTbx.characterLimit = data.CharacterLimit;
            passwordTbx.text = "";

            if (!string.IsNullOrEmpty(data.ValidCharacters))
            {
                passwordTbx.onValidateInput = (string text, int charIndex, char addedChar) =>
                    ValidateChar(data.ValidCharacters, addedChar);
            }

            passwordTbx.Select();
        }
    }


    protected override bool ValidateResult(CreateSignInData result)
    {

        if (!Validator.IsValidLogin(result.Email,result.Password))
        {
            return false;
        }
        return true;

    }
}
