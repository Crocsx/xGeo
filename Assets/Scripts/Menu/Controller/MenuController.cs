using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button startButton;
    public List<PlayerPanel> playerPanel = new List<PlayerPanel>();



    // Use this for initialization
    void Start ()
    {
        CleanPlayerManager();

        InputManager.instance.OnNewController += AddPlayerPannel;

        for (int i = 0; i< InputManager.instance.assignedController.Count; i++)
        {
            AddPlayerPannel(InputManager.instance.assignedController[i]);
        }
        startButton.Select();
    }
	
    void CleanPlayerManager()
    {
        PlayersManager.instance.RemoveAllPlayers();
    }

    void AddPlayerPannel(int controllerID)
    {
        for (int i = 0; i < playerPanel.Count; i++)
        {
            if (!playerPanel[i].isInUse)
            {
                playerPanel[i].Activate(PlayersManager.instance.AddPlayer(controllerID));
                break;
            }
        }
    }
}

