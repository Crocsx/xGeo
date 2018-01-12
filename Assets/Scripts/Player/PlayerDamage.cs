using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour {

    float multiplicator = 0;
    Rigidbody2D _rigidbody;

    void Start()
    {
        _rigidbody = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public void GetDamage(Vector2 dir, float power)
    {
        multiplicator += power / 10;
        _rigidbody.AddForce(dir * power * multiplicator);
    }
}
