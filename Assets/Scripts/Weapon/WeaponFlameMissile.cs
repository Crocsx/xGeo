using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFlameMissile : Usable
{
    float SHOOT_MAX_COOLDOWN = 1.0f;
    int SHOOT_MAX_NUMBER = 5;

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

    public override void Use(Vector3 shootPosition)
    {
        base.Use(shootPosition);
        if (currentCooldown <= 0)
        {
            GameObject missile = Instantiate(flameMissile, transform.position, launcher.transform.rotation);
            missile.GetComponent<Missile>().launcher = launcher;

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
