using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class StartGame : KienBehaviour
{
    private SocketIOUnity socket;
    private GameObject player;
    public User Owner;
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
        socket = NetworkManager.socket;
        player = NetworkManager.Instance.player;
        listenerEventSockeIO();
    }
    private void listenerEventSockeIO()
    {
        socket.OnConnected += (sender, e) =>
        {
            socket.On("game_started", response =>
            {
                Owner = NetworkManager.Instance.Owner;
                var json = response.GetValue().ToString();
                List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
                MainThreadDispatcher.RunOnMainThread(() =>
                {
                    foreach (var user in users)
                    {
                        if(user.id == Owner.id)
                        {
                            CreateUserOnWorld(player, Owner);
                        }
                        else
                        {
                            CreateUserOnWorld(player, user); 
                        }
                    }
                });
            });
        };
    }

    private void CreateUserOnWorld(GameObject player,User user)
    {
        GameObject newUser = Instantiate(player, transform.position, Quaternion.identity);
        KienNetworkBehaviour userScript = newUser.GetComponent<KienNetworkBehaviour>();

        var components = newUser.GetComponents<KienNetworkBehaviour>();
        foreach (KienNetworkBehaviour component in components)
        {
            component.User = user;
            component.NetworkId = user.id;
        }
        userScript.User = user;
        userScript.NetworkId = user.id;
    }
}
