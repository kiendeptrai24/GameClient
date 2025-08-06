using System.Collections.Generic;
using UnityEngine;

public class NetworkObject : KienNetworkBehaviour
{
    public static Dictionary<string, NetworkObject> AllObjects = new();

    private void Awake()
    {
        if (!string.IsNullOrEmpty(NetworkId) && !AllObjects.ContainsKey(NetworkId))
        {
            AllObjects.Add(NetworkId, this);
        }
    }

    void OnDestroy()
    {
        if (!string.IsNullOrEmpty(NetworkId))
        {
            AllObjects.Remove(NetworkId);
        }
    }
}
