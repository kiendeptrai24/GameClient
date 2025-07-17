using SocketIOClient;
using System.Threading.Tasks;
using System;
using UnityEngine;
using Newtonsoft.Json;

public class ChatManager : Singleton<ChatManager>
{
    SocketIOUnity socket;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
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
        socket.On("chat_ack", response =>
        {
            var jsonString = response.GetValue<string>();
            var messege = JsonConvert.DeserializeObject<Messege>(jsonString);
            MainThreadDispatcher.RunOnMainThread(() => { ChatManager_UI.Instance.ShowMessege(messege); });

        });
        socket.On("chat_messege", response =>
        {
            try
            {
                var json = response.GetValue<string>();
                Messege messege = JsonConvert.DeserializeObject<Messege>(json);


                MainThreadDispatcher.RunOnMainThread(() =>
                {
                    ChatManager_UI.Instance.ShowMessege(messege);
                });
            }
            catch (Exception ex)
            {
                Debug.LogError("error in chat_messege handler: " + ex.Message);
            }
        });
    }
    public async Task<Messege> WaitForUserChatAsync(string chatMessege)
    {
        var tcs = new TaskCompletionSource<Messege>();

        Action<SocketIOResponse> handler = null;
        handler = (response) =>
        {
            var json = response.GetValue<string>();
            var messege = JsonConvert.DeserializeObject<Messege>(json);

            socket.Off("chat_ack");
            tcs.TrySetResult(messege);
        };

        socket.On("chat_ack", handler);
        socket.Emit("chat_messege", chatMessege);

        return await tcs.Task;
    }
    public void ChatMessage(string message) => socket.Emit("chat_messege", message);

    protected override void LoadComponent()
    {
        base.LoadComponent();
    }
}
