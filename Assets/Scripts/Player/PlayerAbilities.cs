using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour {

    private Rigidbody2D _rigidbody;

    [Header("Fire")]
    Transform firePoint;
    Usable usableItem;

    [Header("Dash")]
    public const float MAX_BOOST_SPEED = 250;
    public const float MAX_BOOST_COOLDOWN = 3;
    public const float BOOST_COOLDOWN_TIME = 1f;
    public const float BOOST_COOLDOWN_RATE = 10f;
    float boostAvailable = MAX_BOOST_SPEED;
    float boostCurrentCooldown = 0;

    [Header("ShockWave")]
    public const float SHOCKWAVE_COOLDOWN = 3f;
    public const float SHOCKWAVE_DURATION = 0.5f;
    public const float SHOCKWAVE_MAX_STRENGHT = 200;
    public const float SHOCKWAVE_MIN_RADIUS = 0.0f;
    public const float SHOCKWAVE_MAX_RADIUS = 3f;
    public GameObject SHOCKWAVE_ANIMATION;
    float shockWaveCurrentCooldown = 0;

    void Start()
    {
        firePoint = transform.GetChild(0);
        _rigidbody = transform.GetComponent<Rigidbody2D>();
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
        usableItem = Item.GetComponent<Usable>();
        usableItem.launcher = transform.GetComponent<Player>();
        usableItem.OnUsed += ReleaseItem;
    }

    public void ReleaseItem()
    {
        usableItem = null;
    }

    public void UseItem()
    {
        if (usableItem != null)
            usableItem.Use(firePoint.position);
    }
    #endregion

    #region Dash
    public void Dash(Vector2 movement, float intensity)
    {
        if (boostAvailable > 0)
        {
            float consumption = Mathf.Lerp(0, boostAvailable, intensity);
            boostAvailable -= consumption;
            _rigidbody.AddForce(movement * consumption * 10);
            boostCurrentCooldown = MAX_BOOST_COOLDOWN;
        }
    }

    void DashCooldown()
    {
        if (boostAvailable == MAX_BOOST_SPEED)
            return;

        boostCurrentCooldown -= TimeManager.instance.time;
        if (boostCurrentCooldown < 0)
        {
            boostAvailable += BOOST_COOLDOWN_RATE;
        }
    }
    #endregion

    #region ShockWave
    public void ShockWave()
    {
        if (shockWaveCurrentCooldown <= 0)
        {
            shockWaveCurrentCooldown = SHOCKWAVE_COOLDOWN;
            Instantiate(SHOCKWAVE_ANIMATION, transform.position, Quaternion.identity).transform.parent = transform;
            StartCoroutine(ShockWaveEffect());
        }
    }

    IEnumerator ShockWaveEffect()
    {
        float timer = 0;

        List<Transform> IgnoreCollision = new List<Transform>();
        IgnoreCollision.Add(transform);

        while (timer < SHOCKWAVE_DURATION)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), Mathf.Lerp(SHOCKWAVE_MIN_RADIUS, SHOCKWAVE_MAX_RADIUS, timer / SHOCKWAVE_DURATION), 1 << LayerMask.NameToLayer("Player"));
      
            int i = 0;
            while (i < hitColliders.Length)
            {
                Debug.Log(hitColliders);
                if (hitColliders[i].transform.CompareTag("Player") && !IgnoreCollision.Contains(hitColliders[i].transform))
                {
                    Vector2 dir = (hitColliders[i].transform.position - transform.position).normalized;
                    float power = (SHOCKWAVE_MAX_STRENGHT * (timer / SHOCKWAVE_DURATION));
                    hitColliders[i].transform.GetComponent<Player>().Damage(dir, power);
                }
                i++;
            }
            timer += TimeManager.instance.time;
            yield return null;
        }
    }

    void ShockWaveCooldown()
    {
        shockWaveCurrentCooldown -= TimeManager.instance.time;
    }
    #endregion
}
