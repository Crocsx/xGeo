using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPanel : MonoBehaviour {

    public bool isInUse = false;
    public GameObject panelActive;
    public GameObject panelInactive;

    int playerID;

    public void Activate () {
        panelInactive.SetActive(false);
        panelActive.SetActive(true);
        isInUse = true;
    }

    public void Deactivate()
    {
        panelActive.SetActive(false);
        panelInactive.SetActive(true);
        isInUse = false;
    }
}
