using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rewired;

#if UNITY_EDITOR
using UnityEditor;
#endif 

public class MenuController : MonoBehaviour
{
    public Button startButton;
    public List<PlayerPanel> playerPanel = new List<PlayerPanel>();

    private void Awake()
    {
        PlayersManager.instance.OnNewPlayer += AddPlayerPannel;
        PlayersManager.instance.OnRemovePlayer += OnRemovePlayer;
    }

    // Use this for initialization
    void Start ()
    {
        AlreadyConnectedController();
        startButton.Select();
    }

    void OnRemovePlayer(PlayerManager pManager)
    {
        for (int i = 0; i < playerPanel.Count; i++)
        {
            if (playerPanel[i].isInUse && playerPanel[i].playerManager == pManager)
            {
                playerPanel[i].Deactivate();
                break;
            }
        }
    }

    void AlreadyConnectedController()
    {
        for (int i = 0; i < PlayersManager.instance.players.Count; i++)
        {
            AddPlayerPannel(PlayersManager.instance.players[i]);
        }
    }

    void AddPlayerPannel(PlayerManager pManager)
    {
        for (int i = 0; i < playerPanel.Count; i++)
        {
            if (!playerPanel[i].isInUse)
            {
                playerPanel[i].Activate(pManager);
                break;
            }
        }
    }

    private void OnDestroy()
    {
        PlayersManager.instance.OnNewPlayer -= AddPlayerPannel;
        PlayersManager.instance.OnRemovePlayer -= OnRemovePlayer;
    }
}

