using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuIGManager : MonoBehaviour {

    public GameObject IGPlayerPanels;
    public GameObject IGEndGamePanel;
    public Button IGEndGamePanelDefaultButton;
    public GameObject IGPausePanel;
    public Button IGPausePanelDefaultButton;
    public PlayerIGPanel[] PlayerIGPanel;
    public EndGameIGRecapPanel[] PlayerEndGamePanel;

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
        GameManager.instance.OnReloadScene += Reload;
        GameManager.instance.OnPauseGame += ActivateIGPause;
        GameManager.instance.OnResumeGame += DeactivateIGPause;
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

    void Reload(string scene)
    {
        EndGame();
    }

    public void EndGame()
    {
        GameManager.instance.OnPauseGame -= ActivateIGPause;
        GameManager.instance.OnResumeGame -= DeactivateIGPause;
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
        IGEndGamePanel.SetActive(true);
        IGEndGamePanelDefaultButton.Select();
    }

    void DeactivateIGEndGame()
    {
        IGEndGamePanel.SetActive(false);
    }

    void ActivateIGPause()
    {
        IGPausePanel.SetActive(true);
        IGPausePanelDefaultButton.Select();
    }

    void DeactivateIGPause()
    {
        IGPausePanel.SetActive(false);
    }

    public void ResumeGame()
    {
        GameManager.instance.ResumeGame();
    }

    public void ReloadGame()
    {
        GameManager.instance.ReloadScene();
    }

    public void ReturnMenu()
    {
        GameManager.instance.LoadScene("Menu");
    }
}
