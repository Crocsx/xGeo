using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PanelInfo
{
    public string name;
    public GameObject panel;
}

public class MenuManager : MonoBehaviour {

    [Header("Panels")]
    public PanelInfo[] panelInfoList;

    Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();

    void Start ()
    {
		for(int i=0; i < panelInfoList.Length; i++)
        {
            panels.Add(panelInfoList[i].name, panelInfoList[i].panel);
        }
	}

    public void GoTo(string panelName)
    {
        CloseAllPannel();
        GameObject panel;
        if(panels.TryGetValue(panelName, out panel))
        {
            panel.SetActive(true);
        }
    }

    void CloseAllPannel()
    {
        foreach (GameObject panel in panels.Values)
        {
            panel.SetActive(false);
        }
    }

    public void StartMatch(string sceneName)
    {
        if(PlayersManager.instance.players.Count > 0)
            GameManager.instance.LoadScene(sceneName);
    }

    public void LoadScene(string sceneName)
    {
        GameManager.instance.LoadScene(sceneName);
    }
}
