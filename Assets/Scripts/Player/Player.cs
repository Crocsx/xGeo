using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // Events
    public delegate void onResetPlayer();
    public event onResetPlayer OnResetPlayer;

    bool locked;
    public bool isLocked {get{return locked;}}

    // Components
    PlayerAbilities _pAbilities;
    PlayerMovement _pMovement;
    PlayerDamage _pDamage;
    CircleCollider2D _pCollider;
    [HideInInspector]
    public Rigidbody2D _pRigidbody;
    [HideInInspector]
    public PlayerManager _pManager;
    [SerializeField]
    public SpriteRenderer _pRenderer;

    void Start () {
        _pAbilities = transform.GetComponent<PlayerAbilities>();
        _pMovement = transform.GetComponent<PlayerMovement>();
        _pDamage = transform.GetComponent<PlayerDamage>();
        _pRigidbody = transform.GetComponent<Rigidbody2D>();
        _pCollider = transform.GetComponent<CircleCollider2D>();
    }
	
    public void Setup(PlayerManager pManager)
    {
        _pManager = pManager;
        ChangeColor(_pManager.playerColor);
    }

	void Update ()
    {
        if (!isLocked)
            Input();
    }

    public void Die()
    {
        _pManager.PlayerDie();
    }

    public void Drop(GameObject itemPrefab)
    {
        _pAbilities.GetItem(itemPrefab);
    }

    void Input()
    {
        Vector2 dirMoveThumbstick = InputManager.instance.GetThumstickAxis("Joy" + _pManager.playerID + "LeftThumbstickX", "Joy" + _pManager.playerID + "LeftThumbstickY");
        Vector2 rotMoveThumbstick = InputManager.instance.GetThumstickAxis("Joy" + _pManager.playerID + "RightThumbstickX", "Joy" + _pManager.playerID + "RightThumbstickY");
        _pMovement.Movement(dirMoveThumbstick);
        _pMovement.Rotation(rotMoveThumbstick);

        if (InputManager.instance.GetAxis("Joy" + _pManager.playerID + "TriggerLeft") > 0)
            _pAbilities.Dash(dirMoveThumbstick, InputManager.instance.GetAxis("Joy" + _pManager.playerID + "TriggerLeft"));
        
        if (InputManager.instance.GetButton("Joy" + _pManager.playerID + "BumperRight"))
            _pAbilities.ShockWave();

        if (InputManager.instance.GetAxis("Joy" + _pManager.playerID + "TriggerRight") > 0)
            _pAbilities.UseItem(); 
    }

    public void Damage(Vector2 dir, float power)
    {
        _pDamage.GetDamage(dir, power);
    }

    public void ChangeColor(Color color)
    {
        _pRenderer.color = color;
    }

    public void ResetPlayer()
    {
        if (OnResetPlayer != null)
            OnResetPlayer();
    }

    public void Invulnerable()
    {
        _pCollider.enabled = false;
    }

    public void Vulnerable()
    {
        _pCollider.enabled = true;
    }

    public void Lock()
    {
        locked = true;
    }

    public void Unlock()
    {
        locked = false;
    }
}
