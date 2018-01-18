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

    Player player;

    void Awake()
    {
        GameManager.instance.OnStartGame += MatchStart;
    }

    public void MatchStart()
    {
        currentLife = MAX_LIFE;

        Vector3 pos = MatchManager.instance.GetSpawnLocation(playerID);
        player = Instantiate(playerPrefab, pos, Quaternion.identity).GetComponent<Player>();
        player.Setup(this);

        Respawn();
    }

    void Respawn()
    {
        Vector3 pos = MatchManager.instance.GetSpawnLocation(playerID);
        player.transform.position = pos;
        player.Unlock();

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
