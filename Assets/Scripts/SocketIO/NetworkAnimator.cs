using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class NetworkAnimator : KienNetworkBehaviour
{
    [SerializeField] private Animator animator;
    private Dictionary<string, object> lastValues = new();
    public List<string> keys;
    public List<object> values = new List<object>();


    private void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (animator == null)
            return;
        
        foreach (var p in animator.parameters)
        {
            switch (p.type)
            {
                case AnimatorControllerParameterType.Bool:
                    lastValues[p.name] = animator.GetBool(p.name);
                    keys.Add(p.name);
                    values.Add(animator.GetBool(p.name));
                    break;
                case AnimatorControllerParameterType.Float:
                    lastValues[p.name] = animator.GetFloat(p.name);
                    keys.Add(p.name);
                    values.Add(animator.GetFloat(p.name));
                    break;
                case AnimatorControllerParameterType.Int:
                    lastValues[p.name] = animator.GetInteger(p.name);
                    keys.Add(p.name);
                    values.Add(animator.GetInteger(p.name));
                    break;
                case AnimatorControllerParameterType.Trigger:
                    lastValues[p.name] = false;
                    keys.Add(p.name);
                    values.Add(false);
                    break;
            }
        }
        foreach (var item in values)
        {
            Debug.Log(item);
        }
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (animator == null)
            return;
        foreach (var p in animator.parameters)
        {
            bool changed = false;
            object newVal = null;

            switch (p.type)
            {
                case AnimatorControllerParameterType.Bool:
                    newVal = animator.GetBool(p.name);
                    if (!Equals(newVal, lastValues[p.name])) changed = true;
                    break;

                case AnimatorControllerParameterType.Float:
                    newVal = animator.GetFloat(p.name);
                    if (!Mathf.Approximately((float)newVal, (float)lastValues[p.name])) changed = true;
                    break;

                case AnimatorControllerParameterType.Int:
                    newVal = animator.GetInteger(p.name);
                    if (!Equals(newVal, lastValues[p.name])) changed = true;
                    break;

                case AnimatorControllerParameterType.Trigger:
                    if (animator.GetBool(p.name)) // Trigger detection có thể cần custom
                        changed = true;
                    break;
            }

            if (changed)
            {
                lastValues[p.name] = newVal;
                var data = new { id = NetworkId, type = p.type.ToString().ToLower(), name = p.name, val = newVal };
                string json = JsonConvert.SerializeObject(data);
                NetworkManager.socket.Emit("anim_update", json);
            }
        }
    }

    public void ApplyAnimUpdate(string type, string name, object val)
    {
        if (IsOwner) return; // Không áp dụng lại cho owner
        if (animator == null)
            return;
        switch (type)
        {
            case "bool":
                animator.SetBool(name, (bool)val);
                break;
            case "float":
                animator.SetFloat(name, System.Convert.ToSingle(val));
                break;
            case "int":
                animator.SetInteger(name, System.Convert.ToInt32(val));
                break;
            case "trigger":
                animator.SetTrigger(name);
                break;
        }
    }
}
