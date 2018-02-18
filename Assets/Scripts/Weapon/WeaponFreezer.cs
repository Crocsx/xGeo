using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFreezer : Weapon {
    public float FREEZE_TIME = 2.5f;

    public GameObject FreezeMissile;

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
            GameObject missile = Instantiate(FreezeMissile, fireTurret.position, fireTurret.rotation);
            missile.GetComponent<Missile>().launcher = launcher;
            missile.GetComponent<Missile>().FREEZE_TIME = FREEZE_TIME;
            currentCooldown = SHOOT_COOLDOWN;
            _currentShoot--;

            if (currentShoot <= 0)
                base.Used();
        }
    }
}
