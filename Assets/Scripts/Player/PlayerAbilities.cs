using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour {

    private Rigidbody2D _rigidbody;
    private Player _player;

    [Header("Fire")]
    Transform firePoint;

    [Header("Dash")]
    public const float MAX_BOOST_SPEED = 200;
    public const float MAX_BOOST_COOLDOWN = 3;
    public const float BOOST_COOLDOWN_TIME = 1f;
    public const float BOOST_COOLDOWN_RATE = 10f;
    float boostAvailable = MAX_BOOST_SPEED;
    float boostCurrentCooldown = 0;

    [Header("ShockWave")]
    public const float MAX_SHOCKWAVE_STRENGHT = 200;
    public const float SHOCKWAVE_COOLDOWN = 3f;
    float shockWaveCurrentCooldown = 0;

    private void Start()
    {
        firePoint = transform.GetChild(0);
        _rigidbody = transform.GetComponent<Rigidbody2D>();
        _player = transform.GetComponent<Player>();
    }
    private void Update()
    {
        if (_player.playerID == 2)
            return;
        Dash();
        Fire();
    }

    #region Fire
    void Fire()
    {
        /*if (InputManager.instance.GetAxis("Joy" + _player.playerID + "TriggerRight") > 0 && currentWeapon != null)
            currentWeapon.Use(firePoint.position);*/
    }
    #endregion

    #region Dash
    void Dash()
    {
        if (InputManager.instance.GetAxis("Joy" + _player.playerID + "TriggerLeft") > 0 && boostAvailable > 0)
        {
            Vector2 movement = InputManager.instance.GetThumstickAxis("Joy" + _player.playerID + "LeftThumbstickX", "Joy" + _player.playerID + "LeftThumbstickY");
            float consumption = Mathf.Lerp(0, boostAvailable, InputManager.instance.GetAxis("Joy" + _player.playerID + "TriggerLeft"));
            boostAvailable -= consumption;
            _rigidbody.AddForce(movement * consumption * 10);
            boostCurrentCooldown = 0;
        }
        else
        {
            DashCooldown();
        }
    }

    void DashCooldown()
    {
        if (boostAvailable == MAX_BOOST_SPEED)
            return;

        boostCurrentCooldown += TimeManager.instance.time;
        if (boostCurrentCooldown > BOOST_COOLDOWN_TIME)
        {
            boostAvailable += BOOST_COOLDOWN_RATE;
        }
    }
    #endregion

    #region ShockWave
    void ShockWave()
    {
        if (InputManager.instance.GetAxis("Joy" + _player.playerID + "BumperRight") > 0 && shockWaveCurrentCooldown > SHOCKWAVE_COOLDOWN)
        {
            Debug.DrawRay(firePoint.position, transform.right * 10000, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, transform.right);
            if (hit.collider != null)
            {
                Debug.Log(hit.transform.name);
            }
            shockWaveCurrentCooldown = 0;
        }
        else
        {
            ShockWaveCooldown();
        }
    }

    void ShockWaveCooldown()
    {
        shockWaveCurrentCooldown += TimeManager.instance.time;
    }
    #endregion

    #region Drop
    void DropItem()
    {

    }
    #endregion
}
