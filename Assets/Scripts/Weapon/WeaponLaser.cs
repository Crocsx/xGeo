using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLaser : Usable
{
    float SHOOT_MAX_COOLDOWN = 1.0f;
    int SHOOT_MAX_NUMBER = 5;
    int SHOOT_POWER = 90;

    int currentShoot;
    float currentCooldown;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        currentShoot = SHOOT_MAX_NUMBER;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        FireCooldown();
    }

    public override void Use(Vector3 shootPosition)
    {
        base.Use(shootPosition);
        if (currentCooldown <= 0)
        {
            Debug.DrawRay(shootPosition, transform.right * 10000, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(shootPosition, transform.right, 1 << LayerMask.NameToLayer("Player"));
            if (hit.collider != null && (hit.collider.gameObject != launcher.gameObject))
            {
                hit.transform.GetComponent<Player>().Damage((transform.right - hit.transform.position).normalized, SHOOT_POWER);
            }
            currentCooldown = SHOOT_MAX_COOLDOWN;
            currentShoot--;

            if (currentShoot <= 0)
                Used();
        }
    }

    void FireCooldown()
    {
        if (currentCooldown > 0)
            currentCooldown -= TimeManager.instance.time;
    }
}
