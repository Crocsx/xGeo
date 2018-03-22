using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Usable, Damager
{
    public AudioClip soundOnActivation;

    public string damagerName { get { return "Weapon"; } }
    public float SHOOT_COOLDOWN = 1.0f;
    public int SHOOT_MAX_NUMBER = 5;

    public float currentShoot { get { return _currentShoot; } }
    protected int _currentShoot = 0;
    protected float currentCooldown;

    protected override void Start()
    {
        base.Start();
        _currentShoot = SHOOT_MAX_NUMBER;
        currentCooldown = 0;
    }

    protected override void Update()
    {
        base.Update();
        Cooldown();
    }

    void Cooldown()
    {
        if (currentCooldown > 0)
            currentCooldown -= TimeManager.instance.time;
    }

    public void DealDamage(Player reciever, Vector2 dir, float power)
    {
        reciever.GetDamage(dir, power, launcher.pManager);
    }

    protected virtual void Shoot(Transform fireTurret)
    {
        Debug.Log("ici");
        PlaySound(soundOnActivation);
    }

    void PlaySound(AudioClip sClip)
    {
        Debug.Log("la");
        if (soundOnActivation == null)
            return;

        GameObject gameObject = new GameObject();
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sClip;
        audioSource.Play();
    }
}
