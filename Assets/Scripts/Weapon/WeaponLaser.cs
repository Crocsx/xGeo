using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLaser : Usable
{
    const int SHOOT_AVAILABLE = 5;
    const float FIRE_COOLDOWN = 1;
    float fireCurrentCooldown = 0.0f;
    int fireCurrentShoot = 0;

    protected override void Start()
    {
        base.Start();
        Debug.Log("start");
        fireCurrentShoot = SHOOT_AVAILABLE;
        Debug.Log("fireCurrentShoot" + fireCurrentShoot);
    }

    protected override void Update()
    {
        base.Update();
        Debug.Log("Update" + fireCurrentShoot);
    }

    public override void Use(Vector3 shootPosition)
    {
        Debug.Log("Use" + fireCurrentShoot);
        base.Use(shootPosition);
        if (fireCurrentCooldown <= 0)
        {
            Debug.DrawRay(shootPosition, transform.right * 10000, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(shootPosition, transform.right, 1 << LayerMask.NameToLayer("Player"));
            if (hit.collider != null)
            {
                Debug.Log(hit.transform.name);
            }
            fireCurrentCooldown = FIRE_COOLDOWN;
            fireCurrentShoot--;

            if (fireCurrentShoot < 0)
                base.Used();
        }
    }
    
    void FireCooldown()
    {
        if (fireCurrentCooldown > 0)
            fireCurrentCooldown -= TimeManager.instance.time;
    }
}
