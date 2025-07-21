using Mono.CSharp;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json.Nodes;
using UnityEditor;
using UnityEngine;

public class AuthenticationManager : KienBehaviour
{
    private SocketIOUnity socket;
    [SerializeField] private Authentication_UI Authentication_UI;
    protected override void Awake()
    {
        base.Awake();
        LoadComponent();
    }
    protected override void Start()
    {
        AddListenerEvent();
        UserSession.LoadToken();
    }
    private void AddListenerEvent()
    {
        if (NetworkManager.socket == null)
        {
            Debug.Log("Socket not initialized");
            return;
        }

        socket = NetworkManager.socket;

        listenerEventSockeIO();
    }

    private void listenerEventSockeIO()
    {
        socket.OnConnected += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(UserSession.Token))
            {
                Debug.Log("🔑 Token found: " + UserSession.Token);
                //Authentication_UI.SetUpStartGameReaddyUI();
                AuAutoLogin(UserSession.Token);
            }
            socket.On("login_success", response =>
            {
                try
                {
                    var jsonArray = response.ToString();
                    var parsedArray = JArray.Parse(jsonArray);
                    var rawJsonString = parsedArray[0]?.ToString();
                    JObject obj = JObject.Parse(rawJsonString);
                    string token = obj["token"]?.ToString();

                    if (!string.IsNullOrEmpty(token))
                    {
                        MainThreadDispatcher.RunOnMainThread(() => { UserSession.SetToken(token); 
                        Debug.Log("✅ Token: " + token); });
                    
                    }
                    else
                    {
                        Debug.LogWarning("❌ Token missing in response!");
                    }

                }
                catch(Exception ex)
                {
                    Debug.Log(ex);
                }
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
            socket.On("auto_login_result", response =>
            {
                Debug.Log("auto login result " + response.GetValue<string>());
                MainThreadDispatcher.RunOnMainThread(() => { Authentication_UI.SetUpStartGameReaddyUI(); });
            });
            socket.On("user_online", response =>
            {
                Debug.Log("user online: " + response.GetValue<string>());
            });
        };

    }
    public void AuRegister(string value) => socket.Emit("resgister", value);
    public void AuLogin(string value) => socket.Emit("login", value);
    public void AuForgetPassword(string value) => socket.Emit("forget_password", value);
    public void AuResetPassword(string value) => socket.Emit("reset_password", value);
    public void AuLogOut(string value) => socket.Emit("logout", value);
    public void AuAutoLogin(string value)
    {
        if (socket == null)
            Debug.Log("dsadasdas");
        socket.Emit("unity_auto_login", value);
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        Authentication_UI = GetComponent<Authentication_UI>();
    }
}
