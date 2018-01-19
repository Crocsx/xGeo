using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuIGManager : MonoBehaviour {

    public PlayerIGPanel[] PlayerIGPanel;
    public static MenuIGManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
}
