using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour, Damager {

    public string damagerName { get { return "Missile"; } }
    public int MISSILE_POWER = 90;
    public float MISSILE_LIFE_SPAWN = 5;
    public float MISSILE_SPEED = 0.2f;
    public float FREEZE_TIME = 0f;
    public GameObject DESTRUCTION_EFFECT;

    float missileCurrentLife;

    [HideInInspector]
    public Player launcher;

    // Use this for initialization
    void Start ()
    {
        missileCurrentLife = MISSILE_LIFE_SPAWN;
    }

	// Update is called once per frame
	void Update ()
    {
        CheckLife();
        Movement();
	}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == launcher.gameObject)
            return;
        
        if (collider.transform.CompareTag("Player"))
        {
            DealDamage(collider.transform.GetComponent<Player>(), (collider.transform.position - transform.position).normalized, MISSILE_POWER);
            if(FREEZE_TIME > 0)
            {
                collider.transform.GetComponent<Player>().Freeze(FREEZE_TIME);
            }
            Unspawn();
        }

        if ((1 << collider.gameObject.layer) == MatchManager.instance.LAYERMASK_DEADZONE.value)
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


    public void Unspawn()
    {
        if(DESTRUCTION_EFFECT != null)
            Instantiate(DESTRUCTION_EFFECT, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void DealDamage(Player reciever, Vector2 dir, float power)
    {
        reciever.GetDamage(dir, power, launcher.pManager);
    }
}
