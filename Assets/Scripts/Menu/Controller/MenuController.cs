using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button startButton;

    public List<PlayerPanel> playerPanel = new List<PlayerPanel>();

    // Use this for initialization
    void Start () {
        InputManager.instance.OnNewController += AddPlayerPannel;

        for (int i = 0; i< InputManager.instance.assignedController.Count; i++)
        {
            AddPlayerPannel(InputManager.instance.assignedController[i]);
        }
        startButton.Select();
    }
	
    void AddPlayerPannel(int id)
    {
        for (int i = 0; i < playerPanel.Count; i++)
        {
            if (!playerPanel[i].isInUse)
            {
                playerPanel[i].Activate();
                break;
            }
        }
    }
	// Update is called once per frame
	void Update ()
    {
    }
}

