using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour {

    public static PlayersManager instance = null;

    public List<PlayerManager> players = new List<PlayerManager>();
    public GameObject playerManager;

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

    public PlayerManager AddPlayer(int controllerID)
    {
        GameObject newPlayer = Instantiate(playerManager, transform.position, Quaternion.identity);
        newPlayer.transform.parent = transform;
        PlayerManager nPlayer= newPlayer.GetComponent<PlayerManager>();
        nPlayer.playerID = controllerID;
        players.Add(nPlayer);
        return nPlayer;
    }

    public void RemovePlayer(PlayerManager pManager)
    {
        for (int i = 0; i < players.Count; i++) if (pManager == players[i]) players.RemoveAt(i);
    }

    public void RemoveAllPlayers()
    {
        for (var i = 0; i < players.Count; i++)
        {
            Destroy(players[i].gameObject);
        }
        players.Clear();
    }

    public int PlayersStillAlive()
    {
        int alive = 0;
        for (int i = 0; i < players.Count; i++) if (players[i].lifeRemaining > 0) alive++;
        return alive;
    }
}
