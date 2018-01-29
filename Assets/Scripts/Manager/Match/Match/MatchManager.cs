using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public static MatchManager instance;

    [Header("Spawn")]
    public GameObject[] spawnPosition;

    [Header("Arena")]
    public Transform arenaCenter;

    public List<PlayerManager> playersRanking = new List<PlayerManager>();

    [Header("Layers")]
    public LayerMask LAYERMASK_PLAYER;
    public LayerMask LAYERMASK_DEADZONE;
    public LayerMask LAYERMASK_TERRAIN;
    public LayerMask LAYERMASK_GAMEELEMENT;
    public LayerMask LAYERMASK_DEFAULT;

    bool isGameFinish
    {
       get { return (PlayersManager.instance.PlayersStillAlive() <= 1); }
    }

    bool gameStarted = false;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        GameManager.instance.OnReloadScene += LoseReferences;
        GameManager.instance.OnLoadScene += LoseReferences;
    }

    void Start()
    {
        GameManager.instance.InitGame();
    }

    public Vector3 GetSpawnLocation(int i)
    {
        return spawnPosition[i].transform.position;
    }

    public void PlayerAnihilated(PlayerManager player)
    {
        playersRanking.Sort((a, b) => a.lifeRemaining.CompareTo(b.lifeRemaining));

        if (isGameFinish)
            FinishGame();
    }


    void Update()
    {
        if (!gameStarted)
        {
            GameManager.instance.StartGame();
            playersRanking = PlayersManager.instance.players;
            gameStarted = true;
        }
    }

    void FinishGame()
    {
        GameManager.instance.FinishGame();
    }

    public void EndGame()
    {
        MenuIGManager.instance.ReturnMenu();
    }

    void LoseReferences(string name)
    {
        GameManager.instance.OnReloadScene -= LoseReferences;
        GameManager.instance.OnLoadScene -= LoseReferences;
        GameManager.instance.EndGame();
        instance = null;
    }
}
