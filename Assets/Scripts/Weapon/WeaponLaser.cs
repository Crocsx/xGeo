using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLaser : Usable
{
    public float SHOOT_MAX_COOLDOWN = 1.0f;
    public int SHOOT_MAX_NUMBER = 5;
    public int SHOOT_POWER = 200;

    int currentShoot;
    float currentCooldown;
    LineRenderer _lineRenderer;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        currentShoot = SHOOT_MAX_NUMBER;
        _lineRenderer = transform.GetComponent<LineRenderer>();
        _lineRenderer.startColor = launcher.pManager.playerColor;
        _lineRenderer.endColor = launcher.pManager.playerColor;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        FireCooldown();
    }

    public override void Use(Transform fireTurret)
    {
        base.Use(fireTurret);

        if (currentCooldown <= 0)
        {
            LayerMask LayerHitable = (MatchManager.instance.LAYERMASK_PLAYER | 1 << MatchManager.instance.LAYERMASK_TERRAIN);

            RaycastHit2D hit = Physics2D.Raycast(fireTurret.position, fireTurret.right, Mathf.Infinity, LayerHitable);
            if (hit.collider != null  && (hit.collider.CompareTag("Player")))
            {
                hit.transform.GetComponent<Player>().Damage((hit.transform.position - fireTurret.right).normalized, SHOOT_POWER);
            }

            Vector3[] laserPoints = new Vector3[] { fireTurret.position, hit.point };
            StartCoroutine(Laser(laserPoints));

            currentCooldown = SHOOT_MAX_COOLDOWN;
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


    void FireCooldown()
    {
        if (currentCooldown > 0)
            currentCooldown -= TimeManager.instance.time;
    }
}
