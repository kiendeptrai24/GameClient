using UnityEngine;
using DG.Tweening;

public class FadeUI : Singleton<FadeUI>
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration = 1f;
    [SerializeField] private float holdDuration = 1f;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        canvasGroup = GetComponent<CanvasGroup>();
        PlayFadeInOut();
    }
    protected override void Awake()
    {
        base.Awake();
        LoadComponent();
        DontDestroyOnLoad(gameObject);
    }

    public void FadeIn()
    {
        canvasGroup.DOFade(1f, duration).SetEase(Ease.Linear);
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public void FadeOut()
    {
        canvasGroup.DOFade(0f, duration).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
            });
    }
    public void PlayFadeInOut()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, duration)
            .SetEase(Ease.Linear)
            .OnStart(() =>
            {
                canvasGroup.blocksRaycasts = true;
                canvasGroup.interactable = true;
            })
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(holdDuration, () =>
                {
                    canvasGroup.DOFade(0f, duration)
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            canvasGroup.blocksRaycasts = false;
                            canvasGroup.interactable = false;
                        });
                });
            });
    }
}
