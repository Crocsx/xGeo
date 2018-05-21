using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainKillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<xPlayer>().Die();
        }
    }
}
