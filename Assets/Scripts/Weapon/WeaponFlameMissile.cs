using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFlameMissile : Usable
{
    public float SHOOT_MAX_COOLDOWN = 1.0f;
    public int SHOOT_MAX_NUMBER = 5;
    public GameObject flameMissile;

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

    public override void Use(Transform fireTurret)
    {
        base.Use(fireTurret);
        if (currentCooldown <= 0)
        {
            GameObject missile = Instantiate(flameMissile, fireTurret.position, fireTurret.rotation);
            missile.GetComponent<Missile>().launcher = launcher;

            currentCooldown = SHOOT_MAX_COOLDOWN;
            currentShoot--;

            if (currentShoot <= 0)
                base.Used();
        }
    }

    void FireCooldown()
    {
        if (currentCooldown > 0)
            currentCooldown -= TimeManager.instance.time;
    }
}
