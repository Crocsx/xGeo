using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIGPanel : MonoBehaviour {

    public Text playerName;
    public Text playerMultiplicator;
    public RectTransform playerDashCDContainer;
    public RectTransform playerDashCD;
    public RectTransform playerShockWaveCDContainer;
    public RectTransform playerShockWaveCD;

    public bool inUse = false;
    PlayerManager pManager;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        playerName.text = "Player" + pManager.playerID;
        playerName.color = pManager.playerColor;
        playerMultiplicator.text = Mathf.Floor(pManager.player.pDamage.multiplicator).ToString();
        playerDashCD.sizeDelta = new Vector2(Mathf.Lerp(0, playerDashCDContainer.sizeDelta.x, pManager.player.pAbilities.boostAvailable / PlayerAbilities.MAX_BOOST_SPEED), playerDashCDContainer.sizeDelta.y);
        playerShockWaveCD.sizeDelta = new Vector2(Mathf.Lerp(playerShockWaveCDContainer.sizeDelta.x, 0, pManager.player.pAbilities.shockWaveCurrentCooldown / PlayerAbilities.SHOCKWAVE_COOLDOWN), playerDashCDContainer.sizeDelta.y);
    }

    public void Activate(PlayerManager pMgr)
    {
        gameObject.SetActive(true);
        inUse = true;
        pManager = pMgr;
    }
    public void Deactivate(PlayerManager pMgr)
    {
        gameObject.SetActive(false);
        inUse = false;
        pManager = pMgr;
    }

}
