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
    }
    public void setpos(Vector3 pos) => transform.position = pos;
}
