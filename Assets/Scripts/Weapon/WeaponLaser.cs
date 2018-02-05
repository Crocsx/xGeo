using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLaser : Weapon
{
    public int SHOOT_POWER = 200;

    LineRenderer _lineRenderer;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        _lineRenderer = transform.GetComponent<LineRenderer>();
        _lineRenderer.endColor = launcher.pManager.playerColor;
    }

    public override void Use(Transform fireTurret)
    {
        base.Use(fireTurret);

        if (currentCooldown <= 0)
        {
            LayerMask LayerHitable = (MatchManager.instance.LAYERMASK_PLAYER.value | MatchManager.instance.LAYERMASK_TERRAIN.value);

            RaycastHit2D hit = Physics2D.Raycast(fireTurret.position, fireTurret.right, Mathf.Infinity, LayerHitable);

            if (hit.collider != null  && (hit.collider.CompareTag("Player")))
            {
                DealDamage(hit.transform.GetComponent<Player>(), (hit.transform.position - fireTurret.right).normalized, SHOOT_POWER);
            }

            Vector3[] laserPoints = new Vector3[] { fireTurret.position, hit.point };
            StartCoroutine(Laser(laserPoints));

            currentCooldown = SHOOT_COOLDOWN;
            currentShoot--;

            if (currentShoot <= 0)
                base.Used();
        }
    }

    IEnumerator Laser(Vector3[] laserPoints)
    {
        float duration = 0.1f;
        _lineRenderer.SetPositions(laserPoints);
        _lineRenderer.enabled = true;
        while (duration > 0)
        {
            duration -= TimeManager.instance.time;
            yield return null;
        }

        _lineRenderer.enabled = false;
    }
}
