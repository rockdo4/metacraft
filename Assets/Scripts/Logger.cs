using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
public static class Logger
{
    [Conditional("UNITY_EDITOR")]
    public static void Debug(string logMsg)
    {
        UnityEngine.Debug.Log(logMsg);
    }
    [Conditional("UNITY_EDITOR")]
    public static void Debug(int logMsg)
    {
        UnityEngine.Debug.Log(logMsg);
    }
    [Conditional("UNITY_EDITOR")]
    public static void Debug(float logMsg)
    {
        UnityEngine.Debug.Log(logMsg);
    }
}
