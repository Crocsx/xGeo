using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerPanel : MonoBehaviour {

    public bool isInUse = false;
    public GameObject panelActive;
    public GameObject panelInactive;
    public AudioClip playerJoin;
    public AudioClip playerLeft;
    public AudioSource audioSource;
    PlayerManager playerManager;

    public void Activate (PlayerManager pManager)
    {
        playerManager = pManager;
        panelInactive.SetActive(false);
        panelActive.SetActive(true);
        isInUse = true;
        audioSource.clip = playerJoin;
        audioSource.Play();
        SetColor();
    }

    public void Deactivate()
    {
        playerManager = null;
        panelActive.SetActive(false);
        panelInactive.SetActive(true);
        audioSource.clip = playerLeft;
        audioSource.Play();
        isInUse = false;
    }

    void SetColor()
    {
        playerManager.ChangeColor(panelActive.GetComponent<Image>().color);
    }
}
