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

    public bool Tracked;
    public float Track_FOV;
    public float Track_Rotation_Speed;

    float missileCurrentLife;

    [HideInInspector]
    public xPlayer launcher;

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
            DealDamage(collider.transform.GetComponent<xPlayer>(), (collider.transform.position - transform.position).normalized, MISSILE_POWER);
            if(FREEZE_TIME > 0)
            {
                collider.transform.GetComponent<xPlayer>().Freeze(FREEZE_TIME);
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
        Vector3 newPos;

        newPos = transform.right * MISSILE_SPEED * TimeManager.instance.time;

        if (Tracked)
        {       
            GameObject[] players;
            players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {
                if ((Vector3.Angle(transform.right, player.transform.position) < Track_FOV) && player.GetComponent<xPlayer>() != launcher)
                {
                    Vector3 heading = (player.transform.position - transform.position).normalized;

                    RotateToward(heading);

                    newPos = heading * MISSILE_SPEED * TimeManager.instance.time;
                }
            }
        }
        transform.position += newPos;
    }

    void RotateToward(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Slerp(transform.rotation, q, TimeManager.instance.time * Track_Rotation_Speed);
    }

    public void Unspawn()
    {
        if(DESTRUCTION_EFFECT != null)
            Instantiate(DESTRUCTION_EFFECT, transform.position, Quaternion.identity);

        transform.GetComponent<Rigidbody2D>().simulated = false;
        transform.GetComponent<Collider2D>().enabled = false;

        Invoke("DestroyMissile", 2);
    }

    void DestroyMissile()
    {
        Destroy(gameObject);
    }

    public void DealDamage(xPlayer reciever, Vector2 dir, float power)
    {
        reciever.GetDamage(dir, power, launcher.pManager);
    }
}
