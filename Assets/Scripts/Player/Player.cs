using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, Damager, Damageable {

    // Events
    public delegate void onResetPlayer();
    public event onResetPlayer OnResetPlayer;

    public string damagerName { get { return "Player"; } }

    // State
    bool _locked;
    public bool isLocked {get{return _locked;}}

    bool _freezed;
    public bool isFreezed { get { return _freezed; } }

    bool _invulnerable;
    public bool isInvulnerable { get { return _invulnerable; } }

    // Components
    PlayerAbilities _pAbilities;
    public PlayerAbilities pAbilities { get { return _pAbilities; } }
    PlayerMovement _pMovement;
    public PlayerMovement pMovement { get { return _pMovement; } }
    PlayerDamage _pDamage;
    public PlayerDamage pDamage { get { return _pDamage; } }
    Rigidbody2D _pRigidbody;
    public Rigidbody2D pRigidbody { get { return _pRigidbody; } }
    PlayerManager _pManager;
    public PlayerManager pManager { get { return _pManager; } }
    CircleCollider2D _pCollider;
    public CircleCollider2D pCollider { get { return _pCollider; } }

    // Public cause on child and can't get it on instantiation
    [SerializeField]
    public SpriteRenderer pRenderer;
    [SerializeField]
    public TrailRenderer pTrailRight;
    [SerializeField]
    public TrailRenderer pTrailLeft;

    void Start()
    {
        _pAbilities = transform.GetComponent<PlayerAbilities>();
        _pMovement = transform.GetComponent<PlayerMovement>();
        _pDamage = transform.GetComponent<PlayerDamage>();
        _pRigidbody = transform.GetComponent<Rigidbody2D>();
        _pCollider = transform.GetComponent<CircleCollider2D>();

        GameManager.instance.OnPauseGame += Lock;
        GameManager.instance.OnResumeGame += Unlock;
    }
    
	
    public void Setup(PlayerManager pManager)
    {
        _pManager = pManager;
        ChangeColor(_pManager.playerColor);
    }

	void Update ()
    {
        if (!isLocked && !isFreezed)
            Input();
    }

    public void Freeze(float timer)
    {
        _freezed = true;
        StartCoroutine(FreezeTime(timer));
    }

    public void UnFreeze()
    {
        _freezed = false;
    }

    IEnumerator FreezeTime(float timer)
    {
        float currTimer = 0;
        while (currTimer < timer)
        {
            currTimer += TimeManager.instance.time;
            yield return null;
        }

        UnFreeze();
    }

    public void Invulnerable(float timer)
    {
        _invulnerable = true;
        StartCoroutine(InvulnerableTimer(timer));
    }

    public void Vulnerable()
    {
        _invulnerable = false;
    }

    IEnumerator InvulnerableTimer(float timer)
    {
        float currTimer = 0;
        while (currTimer < timer)
        {
            currTimer += TimeManager.instance.time;
            yield return null;
        }

        Vulnerable();
    }

    public void Die()
    {
        if(_pDamage.lastHitter != null)
        {
            _pDamage.lastHitter.HasKilled(_pManager);
        }
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

        if (InputManager.instance.GetButton("Joy" + _pManager.playerID + "Start"))
            GameManager.instance.PauseGame();
    }

    public void GetDamage(Vector2 dir, float power, PlayerManager hitter)
    {
        if(!_invulnerable)
            _pDamage.GetDamage(dir, power, hitter);
    }

    public void DealDamage(Player reciever, Vector2 dir, float power)
    {
        reciever.GetDamage(dir, power, _pManager);
    }

    public void ChangeColor(Color color)
    {
        pTrailRight.startColor = pTrailRight.endColor = color;
        pTrailLeft.startColor = pTrailLeft.endColor = color;
        pRenderer.color = color;
    }

    public void ResetPlayer()
    {
        if (OnResetPlayer != null)
            OnResetPlayer();
    }

    public void Lock()
    {
        _locked = true;
    }

    public void Unlock()
    {
        _locked = false;
    }

    private void OnDestroy()
    {
        GameManager.instance.OnPauseGame -= Lock;
        GameManager.instance.OnResumeGame -= Unlock;
    }
}
