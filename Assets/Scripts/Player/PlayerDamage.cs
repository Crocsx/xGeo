using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour {

    float multiplicator = 0;
    Player _player;

    void Start()
    {
        _player = transform.GetComponent<Player>();
        _player.OnResetPlayer += ResetDamage;
    }

    // Update is called once per frame
    public void GetDamage(Vector2 dir, float power)
    {
        multiplicator += power / 10;
        _player._pRigidbody.AddForce(dir * power * multiplicator);
    }

    void ResetDamage()
    {

    }
}
