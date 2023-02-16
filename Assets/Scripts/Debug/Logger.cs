using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class Logger
{
    [Conditional("EnableLogger")]
    public static void Debug(object logMsg)
    {
        UnityEngine.Debug.Log(logMsg);
    }

    [Conditional("EnableLogger")]
    public static void Debug(object message, Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }

    [Conditional("EnableLogger")]
    public static void Error(object message)
    {
        UnityEngine.Debug.LogError(message);    
    }

    [Conditional("EnableLogger")]
    public static void Error(object message, Object context)
    {
        UnityEngine.Debug.LogError(message, context);
    }
}
