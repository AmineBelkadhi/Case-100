using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public static class NetUtils
{

    
    public static string GetNetworkError(byte error)
    {
        if (error != (byte)NetworkError.Ok)
        {
            NetworkError nerror = (NetworkError)error;
            return nerror.ToString();
        }

        return "";
    }

    
    public static bool IsNetworkError(byte error)
    {
        if (error != (byte)NetworkError.Ok)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsSocketValid(int sock)
    {
        if (sock < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}