using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerManager : MonoBehaviour
{
    public delegate void onPlayerInstantiated();
    public event onPlayerInstantiated OnPlayerInstantiated;
    public delegate void onPlayerRespawned(xPlayer player);
    public event onPlayerRespawned OnPlayerRespawned;
    public delegate void onPlayerKilled(xPlayer player);
    public event onPlayerKilled OnPlayerKilled;

    [Header("Spawn")]
    public float RESPAWN_TIME = 3;

    [Header("Life")]
    public int MAX_LIFE = 1;
    public int lifeRemaining { get { return _lifeRemaining; } }
    int _lifeRemaining = 0;


    [Header("Score")]
    public int pointPerKill = 100;
    public int pointPerDeath = -50;

    [HideInInspector]
    public int kill { get { return _kill; } }
    int _kill = 0;

    [Header("Effects")]
    public GameObject explosion;

    [HideInInspector]
    public int score { get { return _score; } }
    int _score = 0;

    [Header("Params")]
    public Color playerColor;
    public GameObject playerPrefab;

    public xPlayer player { get { return _player; } }
    xPlayer _player;

    public string playerName;

    public int playerID { get { return _playerID; } }
    int _playerID;

    public Player playerController { get { return _playerController; } }
    Player _playerController;

    void Awake()
    {
        GameManager.instance.OnInitGame += GameInit;
        GameManager.instance.OnStartGame += GameStart;
        GameManager.instance.OnFinishGame += GameFinished;
    }

    public void GameInit()
    {
        playerName = "Player " + (_playerID+1);
        MenuIGManager.instance.RequestPanel(this);
        Vector3 pos = MatchManager.instance.GetSpawnLocation(playerID);
        _player = Instantiate(playerPrefab, pos, Quaternion.identity).GetComponent<xPlayer>();
        _player.Setup(this);

        _playerController = ReInput.players.GetPlayer(playerID);

        if (OnPlayerInstantiated != null)
            OnPlayerInstantiated();
    }

    public void reset()
    {
        _lifeRemaining = MAX_LIFE;
        _kill = 0;

        if(_player != null)
            Destroy(_player);
    }

    public void AssignPlayerController(Player pController, int pID)
    {
        _playerController = playerController;
        _playerID = pID;
    }

    void GameStart()
    {
        Respawn();
    }

    public void GameFinished()
    {
        DisablePlayer();
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

    public void HasKilled(PlayerManager kill)
    {
        _score += pointPerKill;
        _kill++;
    }

    void LoseLife()
    {
        _lifeRemaining--;
        _score += pointPerDeath;
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

    void OnDestroy()
    {
        GameManager.instance.OnInitGame -= GameInit;
        GameManager.instance.OnStartGame -= GameStart;
        GameManager.instance.OnFinishGame -= GameFinished;
    }
}
