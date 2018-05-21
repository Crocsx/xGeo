using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public int BOMB_POWER = 90;
    public float BOMB_TIMER = 2;
    public GameObject DESTRUCTION_EFFECT;
    public AudioClip soundExplosion;

    float missileCurrentTimer;

    bool exploded;

    [HideInInspector]
    public xPlayer launcher;

    // Use this for initialization
    void Start()
    {
        missileCurrentTimer = BOMB_TIMER;
        exploded = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckTimer();
    }

    void CheckTimer()
    {
        if (missileCurrentTimer <= 0)
            Explode();

        missileCurrentTimer -= TimeManager.instance.time;
    }

    void Explode()
    {
        if (exploded)
        {
            Unspawn();
            return;
        }

        PlayExplosionSound();

        transform.GetComponent<Collider2D>().enabled = true;
        exploded = true;
        missileCurrentTimer = 0.4f;
    }

    void PlayExplosionSound()
    {
        if (soundExplosion == null)
            return;

        GetComponent<AudioSource>().clip = soundExplosion;
        GetComponent<AudioSource>().Play();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == launcher.gameObject)
            return;

        if (collider.transform.CompareTag("Player"))
        {
            DealDamage(collider.transform.GetComponent<xPlayer>(), (collider.transform.position - transform.position).normalized, BOMB_POWER);
            Unspawn();
        }

        if ((1 << collider.gameObject.layer) == MatchManager.instance.LAYERMASK_DEADZONE.value)
        {
            Unspawn();
        }
    }

    public void Unspawn()
    {
        if (DESTRUCTION_EFFECT != null)
            Instantiate(DESTRUCTION_EFFECT, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void DealDamage(xPlayer reciever, Vector2 dir, float power)
    {
        reciever.GetDamage(dir, power, launcher.pManager);
    }
}
