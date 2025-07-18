using UnityEngine;
using DG.Tweening;

public class Roulette : MonoBehaviour
{
    [Header("Settings")]
    public float rotatePower = 1000f;
    public float stopPower = 300f;
    public float snapDuration = 0.5f;

    [Header("Rewards")]
    public int[] rewardValues = { 100, 200, 300, 400, 500, 600, 700, 800 };

    private Rigidbody2D rbody;
    private RectTransform rectTransform;

    private bool isSpinning = false;
    private float waitAfterStop = 0f;

    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (isSpinning)
        {
            if (rbody.angularVelocity > 0)
            {
                rbody.angularVelocity -= stopPower * Time.deltaTime;
                rbody.angularVelocity = Mathf.Clamp(rbody.angularVelocity, 0, 1440);
            }

            if (Mathf.Approximately(rbody.angularVelocity, 0f))
            {
                waitAfterStop += Time.deltaTime;
                if (waitAfterStop >= 0.5f)
                {
                    waitAfterStop = 0f;
                    isSpinning = false;
                    GetReward();
                }
            }
        }
    }

    public void Spin()
    {
        if (!isSpinning)
        {
            rbody.AddTorque(rotatePower);
            isSpinning = true;
        }
    }

    private void GetReward()
    {
        float angle = rectTransform.eulerAngles.z % 360f;
        int segmentCount = rewardValues.Length;
        float segmentAngle = 360f / segmentCount;
        float halfSegment = segmentAngle / 2f;

        angle = (angle + halfSegment) % 360f;
        int index = Mathf.FloorToInt(angle / segmentAngle);
        index = Mathf.Clamp(index, 0, rewardValues.Length - 1);

        float targetAngle = index * segmentAngle;
        rectTransform.DORotate(new Vector3(0, 0, targetAngle), snapDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutBack);

        Win(rewardValues[index]);
    }

    private void Win(int reward)
    {
        Debug.Log($"🎉 You won: {reward}");
    }
}
