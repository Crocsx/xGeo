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
    public Text life;
    
    public Image WeaponImage;
    public Image WeaponEmpty;
    public Image WeaponRemaining;

    public bool inUse = false;


    Weapon currentWeapon;
    PlayerManager pManager;

    // Use this for initialization
    void Start ()
    {
    }

    void AddEvent()
    {
        pManager.player.GetComponent<PlayerAbilities>().OnItemDrop += DropWeapon;
        pManager.player.GetComponent<PlayerAbilities>().OnItemRelease += ReleaseWeapon;
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
        life.text = pManager.lifeRemaining.ToString();
        
        if (WeaponImage.IsActive())
        {
            Debug.Log(Mathf.Lerp(1, 0, currentWeapon.currentShoot / currentWeapon.SHOOT_MAX_NUMBER));
            WeaponRemaining.fillAmount = Mathf.Lerp(0, 1, currentWeapon.currentShoot / currentWeapon.SHOOT_MAX_NUMBER);
        }
    }

    public void Activate(PlayerManager pMgr)
    {
        gameObject.SetActive(true);
        inUse = true;
        pManager = pMgr;
        pManager.OnPlayerInstantiated += AddEvent;
    }

    public void Deactivate( )
    {
        gameObject.SetActive(false);
        inUse = false;
        pManager.player.pAbilities.OnItemDrop -= DropWeapon;
        pManager.player.pAbilities.OnItemRelease -= ReleaseWeapon;
        pManager.OnPlayerInstantiated -= AddEvent;
        pManager = null;
    }

    void DropWeapon(Usable item)
    {
        currentWeapon = item.GetComponent<Weapon>();
        WeaponImage.gameObject.SetActive(true);
        WeaponImage.sprite = item.icon;
        WeaponRemaining.fillAmount = 1;
    }

    void ReleaseWeapon(Usable item)
    {
        currentWeapon = null;
        WeaponImage.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (pManager != null) {
            pManager.OnPlayerInstantiated -= AddEvent;
            pManager.player.pAbilities.OnItemDrop -= DropWeapon;
            pManager.player.pAbilities.OnItemRelease -= ReleaseWeapon;
        }
    }
}
