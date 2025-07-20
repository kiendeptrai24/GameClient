using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class Authentication_UI : KienBehaviour
{
    [Header("User ")]
    [SerializeField] private Button OnSignGarenaBtn;
    [SerializeField] private Button OnSignGuestBtn;
    protected override void Awake()
    {
        base.Awake();
        OnSignGarenaBtn.onClick.AddListener(() =>
        {
            var popup = PopupManager.Instance.GetPopup<CreateSignInPopup>();
            var data = new BasePopupData("Sign In")
            {
                ValidCharacters = CharacterGroups.AllCharacters,
                CharacterLimit = 20
            };
            popup?.ShowPopup(data,
            onConfirm: (CreateSignInData result) =>
            {
                SignInDTO roomRequest = new SignInDTO(result.Email, result.Password);
                string jsonrequest = JsonConvert.SerializeObject(roomRequest);
                LobbyManager.Instance.CreateLobby(jsonrequest);
                SceneLoader.Instance.LoadScene(SceneName.LobbyReady);
            },
            onCancel: () =>
            {

            });
        });
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        OnSignGarenaBtn = GetComponentsInChildren<Button>(true).FirstOrDefault(btn => btn.gameObject.name == "OnSignGarenaBtn");
        OnSignGuestBtn = GetComponentsInChildren<Button>(true).FirstOrDefault(btn => btn.gameObject.name == "OnSignGuestBtn");
    }
}
