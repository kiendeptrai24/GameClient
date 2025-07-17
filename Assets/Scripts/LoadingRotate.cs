using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class LoadingRotate : MonoBehaviour
{
    [SerializeField] private Image background;

    void Start()
    {
        // Xoay vô hạn
        transform.DORotate(new Vector3(0, 0, -360), 1f, RotateMode.FastBeyond360)
                 .SetEase(Ease.Linear)
                 .SetLoops(-1, LoopType.Restart);

        // Tăng alpha dần
        if (background != null)
        {
            Color startColor = background.color;
            startColor.a = 0f;
            background.color = startColor;

            background.DOFade(.5f, 1f); // Fade alpha lên trong 1s
        }
    }
}