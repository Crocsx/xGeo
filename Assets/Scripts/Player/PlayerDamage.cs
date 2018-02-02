using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour {

    public float multiplicator { get { return _multiplicator; } }
    float _multiplicator = 0;
    Player _player;

    [HideInInspector]
    public PlayerManager lastHitter;

    void Start()
    {
        _player = transform.GetComponent<Player>();
        _player.OnResetPlayer += ResetDamage;
    }

    // Update is called once per frame
    public void GetDamage(Vector2 dir, float power, PlayerManager hitter)
    {
        lastHitter = hitter;
        _multiplicator += power / 10;
        _player.pRigidbody.AddForce(dir * power * _multiplicator);
    }

    void ResetDamage()
    {
        _multiplicator = 0;
    }

    private void OnDestroy()
    {
        _player.OnResetPlayer -= ResetDamage;
    }
}
