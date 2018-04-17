using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuIGManager : MonoBehaviour {

    public GameObject IGPlayerPanels;
    public GameObject IGEndGamePanel;
    public Button IGEndGamePanelDefaultButton;
    public GameObject IGPausePanel;
    public Button IGPausePanelDefaultButton;
    public PlayerIGPanel[] PlayerIGPanel;
    public EndGameIGRecapPanel[] PlayerEndGamePanel;

    public static MenuIGManager instance;
    private Button lastSelectedItem;

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

    public void EndGame()
    {
        GameManager.instance.OnInitGame -= GameInit;
        GameManager.instance.OnFinishGame -= FinishGame;
        GameManager.instance.OnEndGame -= EndGame;
        GameManager.instance.OnPauseGame -= ActivateIGPause;
        GameManager.instance.OnResumeGame -= DeactivateIGPause;
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
        List < PlayerManager > ranking = MatchManager.instance.GetRanking();
        for (var i = (ranking.Count - 1); i >= 0; i--)
        {
            PlayerEndGamePanel[i].Activate(ranking[i]);
        }
    }
    void ActivateIGPlayerPanels()
    {
        IGPlayerPanels.SetActive(true);
    }

    void DeactivateIGPlayerPanels()
    {
        for (int i = 0; i < PlayerIGPanel.Length; i++)
        {
            if (PlayerIGPanel[i].inUse)
            {
                PlayerIGPanel[i].Deactivate();
            }
        }
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

    private void Update()
    {
        if (IGPlayerPanels.activeSelf == false)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
                lastSelectedItem.GetComponent<Button>().Select();
            else
                lastSelectedItem = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        }
    }

    public void SetSelection(Button self)
    {
        lastSelectedItem = self;
    }
}
