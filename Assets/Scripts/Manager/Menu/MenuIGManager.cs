using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuIGManager : MonoBehaviour {

    public GameObject IGPlayerPanels;
    public PlayerIGPanel[] PlayerIGPanel;
    public EndGameIGRecapPanel[] PlayerEndGamePanel;
    public GameObject EndGamePanel;

    public static MenuIGManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this) {
            Destroy(gameObject);
            return;
        }

        GameManager.instance.OnInitGame += GameInit;
        GameManager.instance.OnFinishGame += FinishGame;
        GameManager.instance.OnEndGame += EndGame;
    }

    public void GameInit()
    {
        ActivateIGPlayerPanels();
    }

    public void FinishGame()
    {
        DeactivateIGPlayerPanels();
        ActivateIGEndGame();
        ShowStats();
    }

    public void EndGame()
    {
        GameManager.instance.OnInitGame -= GameInit;
        GameManager.instance.OnFinishGame -= FinishGame;
        GameManager.instance.OnEndGame -= EndGame;
        instance = null;
    }

    public void RequestPanel(PlayerManager pMgr)
    {
        for(int i = 0; i < PlayerIGPanel.Length; i++)
        {
            if (!PlayerIGPanel[i].inUse)
            {
                PlayerIGPanel[i].Activate(pMgr);
                break;
            }
        }
    }

    void ShowStats()
    {
        for(var i = (MatchManager.instance.playersRanking.Count - 1); i >= 0; i--)
        {
            PlayerEndGamePanel[i].Activate(MatchManager.instance.playersRanking[i]);
        }
    }
    void ActivateIGPlayerPanels()
    {
        IGPlayerPanels.SetActive(true);
    }

    void DeactivateIGPlayerPanels()
    {
        IGPlayerPanels.SetActive(false);
    }

    void ActivateIGEndGame()
    {
        EndGamePanel.SetActive(true);
    }

    void DeactivateIGEndGame()
    {
        EndGamePanel.SetActive(false);
    }
}
