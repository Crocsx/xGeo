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
    public const int MAX_LIFE = 4;
    int currentLife;

    [Header("Effects")]
    public GameObject explosion;

    [Header("Params")]
    public int playerID;
    public Color playerColor;
    public GameObject playerPrefab;

    Player _player;
    public Player player {get{return _player;}}

    void Awake()
    {
        GameManager.instance.OnStartGame += MatchStart;
    }

    public void MatchStart()
    {
        currentLife = MAX_LIFE;
        MenuIGManager.instance.RequestPanel(this);
        Vector3 pos = MatchManager.instance.GetSpawnLocation(playerID);
        _player = Instantiate(playerPrefab, pos, Quaternion.identity).GetComponent<Player>();
        player.Setup(this);

        Respawn();
    }

    void Respawn()
    {
        player.transform.position = MatchManager.instance.GetSpawnLocation(playerID);
        Vector3 dir = (player.transform.position - MatchManager.instance.arenaCenter.position).normalized;
        player.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 180);

        player.Unlock();
        player.ResetPlayer();

        if (OnPlayerRespawned!= null)
            OnPlayerRespawned(player);
    }

    public void PlayerDie()
    {
        if (OnPlayerKilled != null)
            OnPlayerKilled(player);

        Instantiate(explosion, player.transform.position, Quaternion.identity);
        player.Lock();
        player.transform.position = new Vector3(2000, 2000, 2000);

        currentLife--;

        if (currentLife <= 0)
        {
            Debug.Log("Player " + playerID + " is anihilated");
            return;
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
}
