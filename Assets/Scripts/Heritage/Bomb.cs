using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public int BOMB_POWER = 90;
    public float BOMB_TIMER = 5;
    public GameObject DESTRUCTION_EFFECT;

    float missileCurrentTimer;

    bool exploded;

    [HideInInspector]
    public Player launcher;

    // Use this for initialization
    void Start()
    {
        Debug.Log("SPawn");
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
        
        transform.GetComponent<Collider2D>().enabled = true;
        exploded = true;
        missileCurrentTimer = 0.3f;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == launcher.gameObject)
            return;

        if (collider.transform.CompareTag("Player"))
        {
            DealDamage(collider.transform.GetComponent<Player>(), (collider.transform.position - transform.position).normalized, BOMB_POWER);
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

    public void DealDamage(Player reciever, Vector2 dir, float power)
    {
        reciever.GetDamage(dir, power, launcher.pManager);
    }
}
