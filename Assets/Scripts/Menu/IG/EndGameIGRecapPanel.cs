using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameIGRecapPanel : MonoBehaviour {

    public Text playerName;
    public Image image;
    public Text playerScore;
    public Text playerKill;
    public Text playerDeath;

    public void Activate(PlayerManager pManager)
    {
        gameObject.SetActive(true);
        playerName.text = "Player " +pManager.playerID;
        playerScore.text = pManager.score.ToString();
        playerKill.text = pManager.kill.ToString();
        playerDeath.text = (pManager.MAX_LIFE - pManager.lifeRemaining).ToString();
        image.color = pManager.playerColor;
    }
}
