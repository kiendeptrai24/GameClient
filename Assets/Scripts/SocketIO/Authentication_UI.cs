using System.Collections;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class Authentication_UI : KienBehaviour
{
    AuthenticationManager authenticationManager;
    [Header("Btn Setup")]
    [SerializeField] private Button OnHomePage;
    [Header("User")]
    [SerializeField] private Button OnSignGarenaBtn;
    [SerializeField] private Button OnSignGuestBtn;
    protected override void Awake()
    {
        base.Awake();
        LoadComponent();
        
        authenticationManager = GetComponent<AuthenticationManager>();
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
                authenticationManager.AuLogin(jsonrequest);
                SetUpStartGameReaddyUI();
            },
            onCancel: () =>
            {

            });
        });
        OnSignGuestBtn.onClick.AddListener(() => { SetUpStartGameReaddyUI(); });
        OnHomePage.onClick.AddListener(() =>
        {
            GameStartManager.Instance.OnStartGameButtonClicked();
        });
        
    }
    private IEnumerator SetUpStartStartGame()
    {

        while (OnSignGarenaBtn == null || OnSignGuestBtn == null || OnHomePage == null)
        {
            Debug.Log("dsadasds");
            yield return null;
        }
        Debug.Log("1");
        OnSignGarenaBtn.gameObject.SetActive(false);
        OnSignGuestBtn.gameObject.SetActive(false);
        OnHomePage.gameObject.SetActive(true);
    }
    public void SetUpStartGameReaddyUI()
    {
        StartCoroutine(SetUpStartStartGame());
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
        OnHomePage = GetComponentsInChildren<Button>(true).FirstOrDefault(btn => btn.gameObject.name == "OnHomePage");
    }
}
