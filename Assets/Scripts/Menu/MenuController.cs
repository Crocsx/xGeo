using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public Text nbPlayerText;
    public Button startButton;
    // Use this for initialization
    void Start () {
        //EventSystemManager.currentSystem.SetSelectedGameObject(theButton);
        startButton.Select();
    }
	
	// Update is called once per frame
	void Update () {
        nbPlayerText.text = InputManager.instance.assignedController.Count.ToString();
    }
}
