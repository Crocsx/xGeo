using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBomb : Weapon
{
    public GameObject Bomb;

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

        GameObject bomb = Instantiate(Bomb, fireTurret.position, fireTurret.rotation);
        bomb.GetComponent<Bomb>().launcher = launcher;

        currentCooldown = SHOOT_COOLDOWN;
        _currentShoot--;

        if (currentShoot <= 0)
            base.Used();
    }
}
