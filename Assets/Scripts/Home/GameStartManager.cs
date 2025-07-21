using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStartManager : Singleton<GameStartManager>
{
    
    [Header("Icon")]
    [SerializeField] private Image wifiOnImg;
    [SerializeField] private Image wifiOffImg;
    [Header("Connect server")]
    [SerializeField] private Transform fadeGo;
    [SerializeField] private Transform loadingGo;
    [SerializeField] private GameObject networkManager;
    private string token = "";
    private const string KEYTOKEN = "token";
    protected override void Awake()
    {
        base.Awake();
        token = PlayerPrefs.GetString(token);
        PlayerPrefs.SetString(KEYTOKEN, token);
        fadeGo.gameObject.SetActive(true);
        StartCoroutine(ConnectInternet());
        
    }
    public void OnStartGameButtonClicked()
    {
        StartCoroutine(ConnectAndLoadGame());
    }
    private IEnumerator ConnectInternet()
    {
        while (!NetworkCheck.IsInternetAvailable())
        {
            yield return null;
        }
        wifiOffImg.gameObject.SetActive(false);
        wifiOnImg.gameObject.SetActive(true);
    }
    private IEnumerator ConnectAndLoadGame()
    {
        networkManager.SetActive(true);
        loadingGo.gameObject.SetActive(true);
        NetworkManager.Instance.OnConnectToServer();

        while (!NetworkManager.Instance.IsConnected)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        LoadingRotate.Instance.Hide();
        FadeUI.Instance.FadeIn(
        () =>
        {
            SceneLoader.Instance.LoadScene(SceneName.GameHome);
        });

    }

    protected override void LoadComponent()
    {
        base.LoadComponent();

        wifiOffImg = GetComponentsInChildren<Image>(true).FirstOrDefault(img => img.gameObject.name == "wifiOffImg");
        wifiOnImg = GetComponentsInChildren<Image>(true).FirstOrDefault(img => img.gameObject.name == "wifiOnImg");

        loadingGo = GetComponentsInChildren<Transform>(true).FirstOrDefault(btn => btn.gameObject.name == "loadingGo");
        fadeGo = GetComponentsInChildren<Transform>(true).FirstOrDefault(btn => btn.gameObject.name == "fadeGo");
        networkManager = FindAnyObjectByType<NetworkManager>().gameObject;
    }

    protected override void Start()
    {
        base.Start();
    }
}
