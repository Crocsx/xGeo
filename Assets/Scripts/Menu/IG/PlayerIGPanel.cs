using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIGPanel : MonoBehaviour {

    public Text playerName;
    public Text playerMultiplicator;
    public Image PlayerShip;
    public RectTransform playerDashCDContainer;
    public Image playerDashCD;
    public RectTransform playerShockWaveCDContainer;
    public Image playerShockWaveCD;

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
        PlayerShip.color = pManager.playerColor;
        playerMultiplicator.text = Mathf.Floor(pManager.player.pDamage.multiplicator).ToString();
        playerDashCD.fillAmount = Mathf.Lerp(0, 1, pManager.player.pAbilities.boostAvailable / PlayerAbilities.MAX_BOOST_SPEED);
        playerShockWaveCD.fillAmount = Mathf.Lerp(1, 0, pManager.player.pAbilities.shockWaveCurrentCooldown / PlayerAbilities.SHOCKWAVE_COOLDOWN);
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
