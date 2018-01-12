using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainKillZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.transform.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<Player>().Die();
        }
    }
}
