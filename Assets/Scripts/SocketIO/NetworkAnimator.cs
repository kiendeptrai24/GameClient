using UnityEngine;
using Newtonsoft.Json;

public class KienNetworkAnimator : KienNetworkBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetBool(string param, bool value)
    {
        animator.SetBool(param, value);
        if (IsOwner)
        {
            var data = new { id = NetworkId, type = "bool", name = param, val = value };
            string json = JsonConvert.SerializeObject(data);
            NetworkManager.socket.Emit("anim_update", json);
        }
    }

    public void SetTrigger(string param)
    {
        animator.SetTrigger(param);
        if (IsOwner)
        {
            var data = new { id = NetworkId, type = "trigger", name = param };
            string json = JsonConvert.SerializeObject(data);
            NetworkManager.socket.Emit("anim_update", json);
        }
    }

    public void ApplyAnimUpdate(string type, string name, bool val = false)
    {
        if (!IsOwner)
        {
            if (type == "bool") animator.SetBool(name, val);
            else if (type == "trigger") animator.SetTrigger(name);
        }
    }
}
