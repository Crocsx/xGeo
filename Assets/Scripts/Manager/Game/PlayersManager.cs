using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayersManager : MonoBehaviour {

    public delegate void onNewPlayer(PlayerManager pManager);
    public event onNewPlayer OnNewPlayer;

    public delegate void onRemovePlayer(PlayerManager pManager);
    public event onRemovePlayer OnRemovePlayer;

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

        ControllerManager.instance.OnNewControllerAssigned += AddPlayer;
        ControllerManager.instance.OnControllerUnAssigned += RemovePlayer;

        DontDestroyOnLoad(transform.gameObject);
    }
    #endregion

    public void AddPlayer(Player player, int controllerID)
    {
        GameObject newPlayer = Instantiate(playerManager, transform.position, Quaternion.identity);
        newPlayer.transform.parent = transform;
        PlayerManager pManager = newPlayer.GetComponent<PlayerManager>();
        pManager.AssignPlayerController(player, controllerID);
        players.Add(pManager);
        if (OnNewPlayer != null)
            OnNewPlayer(pManager);
    }

    public void RemovePlayer(Player player, int controllerID)
    {
        foreach(PlayerManager pManager in players)
        {
            if (pManager.playerID == player.id)
            {
                players.Remove(pManager);
                if (OnRemovePlayer != null)
                    OnRemovePlayer(pManager);
                break;
            }
        }
    }

    public void RemoveIDPlayer(PlayerManager pManager)
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

    public void ResetAllPlayers()
    {
        for (var i = 0; i < players.Count; i++)
        {
            players[i].reset();
        }
    }

    public int PlayersStillAlive()
    {
        int alive = 0;
        for (int i = 0; i < players.Count; i++) if (players[i].lifeRemaining > 0) alive++;
        return alive;
    }
}
