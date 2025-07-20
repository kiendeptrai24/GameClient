using System.Text.Json.Nodes;
using UnityEngine;

public class AuthenticationManager : KienBehaviour
{
    SocketIOUnity socket;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        AddListenerEvent();
    }
    private void AddListenerEvent()
    {
        if (NetworkManager.socket == null)
        {
            Debug.LogError("Socket not initialized");
            return;
        }

        socket = NetworkManager.socket;

        listenerEventSockeIO();

    }
    private void listenerEventSockeIO()
    {
        socket.On("login_success", response =>
        {
            Debug.Log("login success: " + response.GetValue<string>());
        });
        socket.On("login_error", response =>
        {
            Debug.Log("login error: " + response.GetValue<string>());
        });
        socket.On("register success", response =>
        {
            Debug.Log("register success: " + response.GetValue<string>());
        });
        socket.On("register error", response =>
        {
            Debug.Log("register error: " + response.GetValue<string>());
        });
        socket.On("login_success", response =>
        {
            Debug.Log("login success: " + response.GetValue<string>());
        });
        socket.On("forget_password_error", response =>
        {
            Debug.Log("forget password error: " + response.GetValue<string>());
        });
        socket.On("reset password success", response =>
        {
            Debug.Log("reset password success: " + response.GetValue<string>());
        });
        socket.On("reset_password_error", response =>
        {
            Debug.Log("reset password error: " + response.GetValue<string>());
        });
        socket.On("logout_error", response =>
        {
            Debug.Log("logout error: " + response.GetValue<string>());
        });

    }
    public void AuRegister(string value) => socket.Emit("resgister", value);
    public void AuLogin(string value) => socket.Emit("login", value);
    public void AuForgetPassword(string value) => socket.Emit("forget_password", value);
    public void AuResetPassword(string value) => socket.Emit("reset_password", value);
    public void AuLogOut(string value) => socket.Emit("logout", value);

    protected override void LoadComponent()
    {
        base.LoadComponent();
    }
}
