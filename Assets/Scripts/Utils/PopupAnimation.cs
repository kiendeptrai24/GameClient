using UnityEngine;
using DG.Tweening;

public static class PopupAnimation
{
    public static void ShowPopup(RectTransform target, CanvasGroup canvasGroup, float duration = 0.3f)
    {
        target.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        target.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
        canvasGroup.DOFade(1f, duration);
    }
    public static void HidePopup(RectTransform target, CanvasGroup canvasGroup, float duration = 0.2f)
    {
        target.DOScale(Vector3.zero, duration).SetEase(Ease.InBack);
        canvasGroup.DOFade(0f, duration).OnComplete(() =>
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            target.gameObject.SetActive(false);
        });
    }
}
