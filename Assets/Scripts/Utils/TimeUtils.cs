
using System;
using UnityEditor;
using UnityEngine;


public static class TimeUtils
{
    public static string ToDate(string timestampStr)
    {
        long timestampMs = long.Parse(timestampStr);

        DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestampMs).LocalDateTime;

        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
