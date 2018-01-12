using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLaser : MonoBehaviour
{
    public const float FIRE_COOLDOWN = 1;
    float fireCurrentCooldown = 0;

    public void Use(Vector3 shootPosition)
    {
        if (fireCurrentCooldown > FIRE_COOLDOWN)
        {
            Debug.DrawRay(shootPosition, transform.right * 10000, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(shootPosition, transform.right);
            if (hit.collider != null)
            {

                Debug.Log(hit.transform.name);
            }
            fireCurrentCooldown = 0;
        }
        else
        {
            FireCooldown();
        }
    }

    void FireCooldown()
    {
        fireCurrentCooldown += TimeManager.instance.time;
    }
}
