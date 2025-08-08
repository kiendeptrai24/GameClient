using QFSW.QC;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObject : KienNetworkBehaviour
{
    public static Dictionary<string, NetworkObject> AllObjects = new();

    private void Start()
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
