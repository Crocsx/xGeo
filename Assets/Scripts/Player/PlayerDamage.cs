using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour {

    public float multiplicator { get { return _multiplicator; } }
    float _multiplicator = 0;
    xPlayer _player;

    [HideInInspector]
    public PlayerManager lastHitter;

    public ParticleSystem particleFeedbackHit;

    void Start()
    {
        _player = transform.GetComponent<xPlayer>();
        _player.OnResetPlayer += ResetDamage;

        ParticleSystem ps = particleFeedbackHit.GetComponent<ParticleSystem>();
        var particleMain = ps.main;
        particleMain.startColor = _player.pManager.playerColor;
    }

    // Update is called once per frame
    public void GetDamage(Vector2 dir, float power, PlayerManager hitter)
    {
        lastHitter = hitter;
        _multiplicator += power / 10;
        //_player.pRigidbody.AddForce(dir * power * _multiplicator);
        FeedbackHitParticle(dir);
    }

    void FeedbackHitParticle(Vector2 dir)
    {
        particleFeedbackHit.Stop();
        particleFeedbackHit.time = 0;

        ParticleSystem ps = particleFeedbackHit.GetComponent<ParticleSystem>();
        var ParticleShape = ps.shape;
        ParticleShape.rotation = new Vector3(0, -Angle(dir), 0);


        particleFeedbackHit.Play();
    }

    void ResetDamage()
    {
        _multiplicator = 0;
    }

    private void OnDestroy()
    {
        _player.OnResetPlayer -= ResetDamage;
    }

    public static float Angle(Vector2 p_vector2)
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }
}
