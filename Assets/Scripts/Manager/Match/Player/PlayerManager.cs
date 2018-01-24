using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public delegate void onPlayerRespawned(Player player);
    public event onPlayerRespawned OnPlayerRespawned;
    public delegate void onPlayerKilled(Player player);
    public event onPlayerKilled OnPlayerKilled;

    [Header("Spawn")]
    public float RESPAWN_TIME = 3;

    [Header("Life")]
    public int MAX_LIFE = 1;
    public int lifeRemaining { get { return _lifeRemaining; } }
    int _lifeRemaining = 0;

    [Header("Effects")]
    public GameObject explosion;

    [Header("Params")]
    public int playerID;
    public Color playerColor;
    public GameObject playerPrefab;

    public Player player { get { return _player; } }
    Player _player;


    void Awake()
    {
        GameManager.instance.OnInitGame += GameInit;
        GameManager.instance.OnStartGame += GameStart;
        GameManager.instance.OnFinishGame += GameFinished;
        GameManager.instance.OnEndGame += GameEnd;
    }

    public void GameInit()
    {
        _lifeRemaining = MAX_LIFE;
        MenuIGManager.instance.RequestPanel(this);
        Vector3 pos = MatchManager.instance.GetSpawnLocation(playerID);
        _player = Instantiate(playerPrefab, pos, Quaternion.identity).GetComponent<Player>();
        _player.Setup(this);
    }

    void GameStart()
    {
        Respawn();
    }

    public void GameFinished()
    {
        DisablePlayer();
    }

    public void GameEnd()
    {
        GameManager.instance.OnInitGame -= GameInit;
        GameManager.instance.OnStartGame -= GameStart;
        GameManager.instance.OnFinishGame -= GameFinished;
        GameManager.instance.OnEndGame -= GameEnd;
    }

    void Respawn()
    {
        _player.transform.position = MatchManager.instance.GetSpawnLocation(playerID);
        Vector3 dir = (_player.transform.position - MatchManager.instance.arenaCenter.position).normalized;
        _player.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 180);

        _player.Unlock();
        _player.ResetPlayer();

        if (OnPlayerRespawned!= null)
            OnPlayerRespawned(player);
    }

    public void PlayerDie()
    {
        if (OnPlayerKilled != null)
            OnPlayerKilled(player);

        Instantiate(explosion, player.transform.position, Quaternion.identity);
        DisablePlayer();

        LoseLife();
    }

    void LoseLife()
    {
        _lifeRemaining--;

        if (_lifeRemaining <= 0)
        {
            MatchManager.instance.PlayerAnihilated(this);
        }
        else
        {
            StartCoroutine(RespawnCountdown());
        }
    }

    IEnumerator RespawnCountdown()
    {
        float timer = 0;

        while (timer < RESPAWN_TIME)
        {
            timer += TimeManager.instance.time;
            yield return null;
        }
        Respawn();
    }

    public void ChangeColor(Color color)
    {
        playerColor = color;
    }

    public void DisablePlayer()
    {
        _player.Lock();
        _player.transform.position = new Vector3(2000, 2000, 2000);
    }
}
