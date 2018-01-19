﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

    public int MISSILE_POWER = 90;
    public float MISSILE_LIFE_SPAWN = 5;
    public float MISSILE_SPEED = 0.2f;
    public GameObject DESTRUCTION_EFFECT;

    float missileCurrentLife;

    [HideInInspector]
    public Player launcher;

    // Use this for initialization
    void Start () {
        missileCurrentLife = MISSILE_LIFE_SPAWN;
    }

	// Update is called once per frame
	void Update () {
        CheckLife();
        Movement();
	}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.CompareTag("Player") && (collider.gameObject != launcher.gameObject))
        {
            collider.transform.GetComponent<Player>().Damage((collider.transform.position - transform.position).normalized, MISSILE_POWER);
            Unspawn();
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("DeadZone"))
        {
            Unspawn();
        }
    }

    void CheckLife()
    {
        if (missileCurrentLife <= 0)
            Unspawn();

        missileCurrentLife -= TimeManager.instance.time;
    }

    void Movement()
    {
        transform.position += transform.right * MISSILE_SPEED * TimeManager.instance.time;
    }


    void Unspawn()
    {
        Instantiate(DESTRUCTION_EFFECT, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
