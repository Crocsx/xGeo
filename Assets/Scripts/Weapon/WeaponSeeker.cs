using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSeeker : Weapon
{
    public GameObject weaponSeeker;
    public int nbSeeker = 3;
    public float delayMissile = 0.5f;


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

        StartCoroutine(ShootSeekers(fireTurret));
        currentCooldown = SHOOT_COOLDOWN;
    }

    private IEnumerator ShootSeekers(Transform fireTurret)
    {
        float timer = 0;
        int spawned = 0;
        while (spawned < nbSeeker)
        {
            if(timer <= 0)
            {
                GameObject missile = Instantiate(weaponSeeker, fireTurret.position, fireTurret.rotation);
                missile.GetComponent<Missile>().launcher = launcher;
                timer = delayMissile;
                spawned++;
            }

            timer -= TimeManager.instance.time;

            yield return null;
        }

        _currentShoot--;

        if (currentShoot <= 0)
            base.Used();
    }
}
