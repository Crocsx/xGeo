using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHammer : Weapon
{

    public float HAMMER_DURATION = 5f;
    public float HAMMER_MIN_RADIUS;
    public float HAMMER_MAX_RADIUS;
    public float HAMMER_MIN_POWER;
    public float HAMMER_MAX_POWER;
    public GameObject FXEffect;

    float currDuration;

    protected override void Start()
    {
        base.Start();
        currentShoot = SHOOT_MAX_NUMBER;
        currDuration = 0;
    }

    protected override void Update()
    {
        base.Update();
        currDuration += TimeManager.instance.time;
    }

    public override void Use(Transform fireTurret)
    {
        base.Use(fireTurret);
        Instantiate(FXEffect, transform.position, Quaternion.identity).transform.parent = transform;
        HammerShoot();
    }

    void HammerShoot()
    {
        float radius = Mathf.Lerp(HAMMER_MIN_RADIUS, HAMMER_MAX_RADIUS, currDuration / HAMMER_DURATION);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), radius, MatchManager.instance.LAYERMASK_PLAYER.value);

        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].transform.CompareTag("Player") && transform != hitColliders[i].transform)
            {
                Vector2 dir = (hitColliders[i].transform.position - transform.position).normalized;
                float power = Mathf.Lerp(HAMMER_MIN_POWER, HAMMER_MAX_POWER, currDuration / HAMMER_DURATION);
                hitColliders[i].transform.GetComponent<Player>().Damage(dir, power);
            }
            i++;
        }
    }
}
