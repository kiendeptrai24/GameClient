using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HomeManager : KienBehaviour
{
    [SerializeField] private Button OnLobbyBtn;
    protected override void Awake()
    {
        base.Awake();
        OnLobbyBtn.onClick.AddListener(() => SceneLoader.Instance.LoadScene(SceneName.Lobby));
    }

    protected override void Start()
    {
        base.Start();
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        OnLobbyBtn = GetComponentsInChildren<Button>().FirstOrDefault(btn => btn.gameObject.name == "OnLobbyBtn");
    }
}
