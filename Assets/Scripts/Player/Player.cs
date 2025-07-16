using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Splines;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    public Transform cameraTransform;
    public float speed = 5f;
    public float rotationSpeed = 720f;

    private CharacterController controller;
    private Vector3 moveDirection;
    private float _turnSmoothVelocity;
    [SerializeField] private NetworkManager networkManager;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(h*10, 0f, v * 10);
        networkManager?.getposition(inputDir);
        //if (inputDir.magnitude >= 0.1f)
        //{
        //    float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        //    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, 0.1f);
        //    transform.rotation = Quaternion.Euler(0f, angle, 0f);

        //    moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * speed;
        //}
        //else
        //{
        //    moveDirection.x = 0;
        //    moveDirection.z = 0;
        //}

        //if (!controller.isGrounded)
        //    moveDirection.y += Physics.gravity.y * Time.deltaTime;
        //else
        //    moveDirection.y = -2f;

        //controller.Move(moveDirection * Time.deltaTime);
    }
    public void setpos(Vector3 pos) => transform.position = pos;
}
