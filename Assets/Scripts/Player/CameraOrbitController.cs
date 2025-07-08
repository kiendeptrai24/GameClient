using UnityEngine;

public class CameraOrbitController : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;           // Nhân vật để xoay quanh
    public Vector3 offset = new Vector3(0, 2, -4); // Khoảng cách so với target

    [Header("Rotation Settings")]
    public float rotationSpeed = 5f;
    public float verticalMin = -30f;
    public float verticalMax = 60f;

    private float currentYaw = 0f;
    private float currentPitch = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        // Lấy input chuột
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Xử lý xoay
        currentYaw += mouseX * rotationSpeed;
        currentPitch -= mouseY * rotationSpeed;
        currentPitch = Mathf.Clamp(currentPitch, verticalMin, verticalMax);

        // Tính vị trí camera mới
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        Vector3 desiredPosition = target.position + rotation * offset;

        // Gán vị trí và hướng nhìn
        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * 1.5f); // Nhìn lên một chút
    }
}
