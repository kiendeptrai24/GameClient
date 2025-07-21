using UnityEngine;

public static class UserSession
{
    public static string Token { get; private set; }

    public static void SetToken(string token)
    {
        Token = token;
        PlayerPrefs.SetString("TOKEN", token);
        PlayerPrefs.Save();
    }

    public static void LoadToken()
    {
        Token = PlayerPrefs.GetString("TOKEN", string.Empty);

    }
}
