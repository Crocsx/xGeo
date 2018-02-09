using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    Player _player;

    [Header("Fire")]
    public Transform fireTurret;
    Usable storedItem;

    [Header("Dash")]
    public const float MAX_BOOST_SPEED = 400;
    public const float MAX_BOOST_COOLDOWN = 3;
    public const float BOOST_COOLDOWN_TIME = 1f;
    public const float BOOST_COOLDOWN_RATE = 10f;
    float _boostAvailable = 0;
    public float boostAvailable { get { return _boostAvailable; } }
    float _boostCurrentCooldown = 0;
    public float boostCurrentCooldown { get { return _boostCurrentCooldown; } }

    [Header("ShockWave")]
    public const float SHOCKWAVE_COOLDOWN = 3f;
    public const float SHOCKWAVE_DURATION = 0.5f;
    public const float SHOCKWAVE_MAX_STRENGHT = 200;
    public const float SHOCKWAVE_MIN_RADIUS = 0.0f;
    public const float SHOCKWAVE_MAX_RADIUS = 6f;
    public GameObject SHOCKWAVE_ANIMATION;

    float _shockWaveCurrentCooldown = 0;
    public float shockWaveCurrentCooldown { get { return _shockWaveCurrentCooldown; } }


    void Start()
    {
        _player = transform.GetComponent<Player>();
        _player.OnResetPlayer += ResetAbilities;

        // Wierd Unity stuff https://docs.unity3d.com/ScriptReference/ParticleSystem.MainModule-startColor.html
        ParticleSystem ShockwaveEffect = SHOCKWAVE_ANIMATION.GetComponent<ParticleSystem>();
        var main = ShockwaveEffect.main;
        main.startColor = _player.pManager.playerColor;
    }

    void Update()
    {
        DashCooldown();
        ShockWaveCooldown();
    }

    #region Item
    public void GetItem(GameObject droppedItem)
    {

        GameObject Item = Instantiate(droppedItem, transform.position, Quaternion.identity);
        Item.transform.parent = transform;
        Usable ItemUsable = Item.GetComponent<Usable>();

        if (ItemUsable.AUTOMATIC_USE)
            InstantActivateItem(ItemUsable);
        else
            StoreItem(ItemUsable);
    }

    void InstantActivateItem(Usable ItemUsable)
    {
        ItemUsable.launcher = _player;
        ItemUsable.Use(fireTurret);
    }

    void StoreItem(Usable ItemUsable)
    {
        if (storedItem)
            storedItem.Used();

        ItemUsable.launcher = _player;
        ItemUsable.OnUsed += ReleaseItem;
        storedItem = ItemUsable;
    }

    public void ReleaseItem()
    {
        storedItem.OnUsed -= ReleaseItem;
        storedItem = null;
    }

    public void UseItem()
    {
        if (storedItem != null)
            storedItem.Use(fireTurret);
    }
    #endregion

    #region Dash
    public void Dash(Vector2 movement, float intensity)
    {
        if (boostAvailable > 0)
        {
            float consumption = Mathf.Lerp(0, boostAvailable, intensity);
            _boostAvailable -= consumption;
            _player.pRigidbody.AddForce(movement * consumption * 10);
            _boostCurrentCooldown = MAX_BOOST_COOLDOWN;
        }
    }

    void DashCooldown()
    {
        if (boostAvailable == MAX_BOOST_SPEED)
            return;

        _boostCurrentCooldown -= TimeManager.instance.time;
        if (boostCurrentCooldown < 0)
        {
            _boostAvailable += BOOST_COOLDOWN_RATE;
        }
    }
    #endregion

    #region ShockWave
    public void ShockWave()
    {
        if (shockWaveCurrentCooldown <= 0)
        {
            _shockWaveCurrentCooldown = SHOCKWAVE_COOLDOWN;
            Instantiate(SHOCKWAVE_ANIMATION, transform.position, Quaternion.identity).transform.parent = transform;
            StartCoroutine(ShockWaveEffect());
        }
    }

    IEnumerator ShockWaveEffect()
    {
        float timer = 0;

        while (timer < SHOCKWAVE_DURATION)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), Mathf.Lerp(SHOCKWAVE_MIN_RADIUS, SHOCKWAVE_MAX_RADIUS, timer / SHOCKWAVE_DURATION), MatchManager.instance.LAYERMASK_PLAYER.value);

            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].transform.CompareTag("Player") && transform != hitColliders[i].transform)
                {
                    Vector2 dir = (hitColliders[i].transform.position - transform.position).normalized;
                    float power = (SHOCKWAVE_MAX_STRENGHT * (timer / SHOCKWAVE_DURATION));
                    _player.DealDamage(hitColliders[i].transform.GetComponent<Player>(), dir, power);
                }
                i++;
            }
            timer += TimeManager.instance.time;
            yield return null;
        }
    }

    void ShockWaveCooldown()
    {
        _shockWaveCurrentCooldown -= TimeManager.instance.time;
    }
    #endregion

    #region Reset
    void ResetAbilities()
    {
        _boostAvailable = MAX_BOOST_SPEED;
        _boostCurrentCooldown = 0;
        _shockWaveCurrentCooldown = 0;
        storedItem = null;
    }
    #endregion
    private void OnDestroy()
    {
        _player.OnResetPlayer -= ResetAbilities;
    }
}
