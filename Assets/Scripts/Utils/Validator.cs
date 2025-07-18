using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;


public class Validator : MonoBehaviour
{

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            Debug.LogWarning("Email is empty!");
            return false;
        }

        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(email, pattern))
        {
            Debug.LogWarning("Email format is invalid!");
            return false;
        }

        return true;
    }

    public static bool IsValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            Debug.LogWarning("Password is empty!");
            return false;
        }

        // Optionally: check độ dài tối thiểu
        if (password.Length < 6)
        {
            Debug.LogWarning("Password must be at least 6 characters!");
            return false;
        }

        return true;
    }

    public static bool IsValidLogin(string email, string password)
    {
        return IsValidEmail(email) && IsValidPassword(password);
    }
}
