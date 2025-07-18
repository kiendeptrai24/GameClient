using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStartManager : KienBehaviour
{
    [SerializeField] private Button OnHomePage;
    [SerializeField] private Transform Loading;
    [SerializeField] private GameObject networkManager;
    protected override void Awake()
    {
        base.Awake();
        OnHomePage.onClick.AddListener(() =>
        {
            OnStartGameButtonClicked();
        });
    }
    public void OnStartGameButtonClicked()
    {
        StartCoroutine(ConnectAndLoadGame());
    }
    private IEnumerator ConnectAndLoadGame()
    {
        Loading.gameObject.SetActive(true);
        networkManager.SetActive(true);
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
        OnHomePage = GetComponentsInChildren<Button>().FirstOrDefault(btn => btn.gameObject.name == "OnHomePage");
    }

    protected override void Start()
    {
        base.Start();
    }
}
