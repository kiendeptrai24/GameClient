using UnityEngine;
using System.Net.NetworkInformation;

public static class NetworkCheck
{
    public static bool IsInternetAvailable()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
}
