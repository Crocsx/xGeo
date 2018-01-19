using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour {

    float _multiplicator = 0;
    public float multiplicator { get { return _multiplicator; } }
    Player _player;

    void Start()
    {
        _player = transform.GetComponent<Player>();
        _player.OnResetPlayer += ResetDamage;
    }

    // Update is called once per frame
    public void GetDamage(Vector2 dir, float power)
    {
        _multiplicator += power / 10;
        _player.pRigidbody.AddForce(dir * power * multiplicator);
    }

    void ResetDamage()
    {
        _multiplicator = 0;
    }
}
