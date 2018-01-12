using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSlowZone : MonoBehaviour {

    public Transform ArenaCenter;
    public float centerForce;

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.transform.CompareTag("Player"))
        {
            Rigidbody2D pRigidBody = collider.gameObject.GetComponent<Rigidbody2D>();
            pRigidBody.AddForce(centerForce * (ArenaCenter.position - collider.transform.position).normalized);
        }
    }
}
