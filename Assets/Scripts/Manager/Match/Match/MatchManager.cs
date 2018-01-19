using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject[] spawnPosition;

    [Header("Arena")]
    public Transform arenaCenter;

    public static MatchManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

    }
    void Start()
    {
        GameManager.instance.StartGame();
    }

    public Vector3 GetSpawnLocation(int i)
    {
        return spawnPosition[i].transform.position;
    }
}
