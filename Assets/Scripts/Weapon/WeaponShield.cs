﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShield : Usable
{
    public float SHIELD_DURATION = 5.0f;
    bool enable = false;
    float currDuration = 0;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        CheckCooldown();
        base.Update();
    }

    public override void Use(Transform fireTurret)
    {
        currDuration = 0;
        enable = true;
        launcher.Invulnerable(SHIELD_DURATION);
        GetComponent<CircleCollider2D>().enabled = true;
    }

    void CheckCooldown()
    {
        if (!enable)
            return;

        currDuration += TimeManager.instance.time;
        if(currDuration >= SHIELD_DURATION)
            base.Used();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<Missile>())
        {
            Missile missile = collision.transform.GetComponent<Missile>();
            if (missile.launcher != launcher) 
                missile.Unspawn();
            
        }
    }
}