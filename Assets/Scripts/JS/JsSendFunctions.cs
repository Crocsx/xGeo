using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class JsSendFunctions : MonoBehaviour
{
    public static JsSendFunctions instance = null;

    [DllImport("__Internal")]
    private static extern void SpawnPowerUP(int x, int y, string name);

    [DllImport("__Internal")]
    private static extern void CameraPosition(int x, int y);

    #region Singleton Initialization 
    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(transform.gameObject);
    }
    #endregion

    void SendSpawnPowerUp(int x, int y, string name)
    {
        SpawnPowerUP(x, y, name);
    }

    void SendCameraPosition(int x, int y)
    {
        CameraPosition(x, y);
    }
}
