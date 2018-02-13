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

    [Header("Layers")]
    public LayerMask LAYERMASK_PLAYER;
    public LayerMask LAYERMASK_DEADZONE;
    public LayerMask LAYERMASK_TERRAIN;
    public LayerMask LAYERMASK_GAMEELEMENT;
    public LayerMask LAYERMASK_OBSTACLE;
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
        if (isGameFinish)
            FinishGame();
    }

    void Update()
    {
        if (!gameStarted)
        {
            GameManager.instance.StartGame();
            gameStarted = true;
        }
    }

    public List<PlayerManager> GetRanking()
    {
        List<PlayerManager> ranking = PlayersManager.instance.players;
        ranking.Sort((a, b) => b.score.CompareTo(a.score));
        return ranking;
    }

    public void FinishGame()
    {
        GameManager.instance.FinishGame();
    }

    public void EndGame()
    {
        if (GameManager.getState() == GameManager.GameState.PAUSE)
            GameManager.instance.ResumeGame();
        GameManager.instance.EndGame();
    }

    public void ReloadGame()
    {
        EndGame();
        GameManager.instance.ReloadScene();
    }

    public void ResumeGame()
    {
        GameManager.instance.ResumeGame();
    }

    public void ReturnMenu()
    {
        EndGame();
        GameManager.instance.LoadScene("Menu");
    }

    void LoseReferences(string name)
    {
        GameManager.instance.OnReloadScene -= LoseReferences;
        GameManager.instance.OnLoadScene -= LoseReferences;
        instance = null;
    }
}
