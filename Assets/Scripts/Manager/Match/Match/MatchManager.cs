using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public Player player;
    public int id;
    public int life;
    public float score;
}

public class MatchManager : MonoBehaviour
{
    public delegate void onMatchStart();
    public event onMatchStart OnMatchStart;
    public delegate void onPlayerKilled(Player player);
    public event onPlayerKilled OnPlayerKilled;
    public delegate void onPlayerSpawned(Player player);
    public event onPlayerSpawned OnPlayerSpawned;

    [Header("Player")]
    public const int MAX_PLAYER_NUMBER = 4;
    public GameObject playerPrefab;

    [Header("Spawn")]
    public GameObject[] spawnPosition;

    [Header("Life")]
    public const int MAX_LIFE_PER_PLAYER = 4;


    Dictionary<int, PlayerInfo> playerList = new Dictionary<int, PlayerInfo>();

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
        MatchStart();
    }

    public void MatchStart()
    {
        InitialPlayerSetup(InputManager.instance.assignedController);
        if (OnMatchStart != null)
            OnMatchStart();
    }

    public void InitialPlayerSetup(List<int> playerIds)
    {
        for (var i = 0; i < playerIds.Count; i++)
        {
            PlayerInfo newPlayer = new PlayerInfo();
            newPlayer.player = InstantiatePlayer(playerIds[i]);
            newPlayer.life = MAX_LIFE_PER_PLAYER;
            newPlayer.score = 0;
            newPlayer.id = playerIds[i];
            playerList.Add(playerIds[i], newPlayer);
        }
    }

    Player InstantiatePlayer(int id)
    {
        Vector3 pos = GetSpawnLocation(id);
        Player nPlayer = Instantiate(playerPrefab, pos, Quaternion.identity).GetComponent<Player>();
        nPlayer.playerID = id;

        if (OnPlayerSpawned != null)
            OnPlayerSpawned(nPlayer);

        return null;
    }

    Vector3 GetSpawnLocation(int i)
    {
        return spawnPosition[i].transform.position;
    }

    public void PlayerDie(Player deadPlayer)
    {
        if (OnPlayerKilled != null)
            OnPlayerKilled(deadPlayer);

        Destroy(deadPlayer.gameObject);

        PlayerInfo pInfo;
        if (playerList.TryGetValue(deadPlayer.playerID, out pInfo))
        {
            Debug.Log("pInfo.life"+ pInfo.life);
            pInfo.life--;

            if (pInfo.life <= 0)
            {
                Debug.Log("Player " + pInfo.id + " is anihilated");
                return;
            }

            pInfo.player = InstantiatePlayer(pInfo.id);
        }
        else
        {
            Debug.Log("FAIL");
        }
    }
}
