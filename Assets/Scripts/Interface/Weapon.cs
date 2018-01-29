﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Usable {

    public float SHOOT_COOLDOWN = 1.0f;
    public int SHOOT_MAX_NUMBER = 5;

    protected int currentShoot;
    protected float currentCooldown;

    protected override void Start()
    {
        base.Start();
        currentShoot = SHOOT_MAX_NUMBER;
        currentCooldown = 0;
    }

    protected override void Update()
    {
        base.Update();
        Cooldown();
    }

    void Cooldown()
    {
        if (currentCooldown > 0)
            currentCooldown -= TimeManager.instance.time;
    }
}
