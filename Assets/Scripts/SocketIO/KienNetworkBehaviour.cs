using UnityEngine;

public class KienNetworkBehaviour : MonoBehaviour
{
    public string NetworkId { get; set; }
    public User User { get; set; }
    public bool IsOwner => User != null && User.isLocal;

    public virtual void OnNetworkSpawn() { }
    public virtual void OnNetworkDespawn() { }
}
