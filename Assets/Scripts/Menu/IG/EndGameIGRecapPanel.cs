using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameIGRecapPanel : MonoBehaviour {

    public Text playerName;
    public Image image;

    public void Activate(PlayerManager pManager)
    {
        gameObject.SetActive(true);
        playerName.text = "Player " +pManager.playerID;
        image.color = pManager.playerColor;
    }
}
