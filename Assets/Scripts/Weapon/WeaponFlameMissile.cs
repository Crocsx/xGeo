using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFlameMissile : Weapon
{
    public GameObject flameMissile;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    public override void Use(Transform fireTurret)
    {
        base.Use(fireTurret);
        if (currentCooldown <= 0)
        {
            Shoot(fireTurret);
        }
    }

    protected override void Shoot(Transform fireTurret)
    {
        base.Shoot(fireTurret);

        GameObject missile = Instantiate(flameMissile, fireTurret.position, fireTurret.rotation);
        missile.GetComponent<Missile>().launcher = launcher;

        currentCooldown = SHOOT_COOLDOWN;
        _currentShoot--;

        if (currentShoot <= 0)
            base.Used();
    }
}
