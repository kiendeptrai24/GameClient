using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStartManager : KienBehaviour
{
    [SerializeField] private Button OnHomePage;
    protected override void Awake()
    {
        base.Awake();
        OnHomePage.onClick.AddListener(() => SceneLoader.Instance.LoadScene(SceneName.GameHome));
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
