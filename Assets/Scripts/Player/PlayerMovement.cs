using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D _rigidbody;

    [Header("Move")]
    public const float MAX_MOVE_SPEED = 180;

    [Header("Rotation")]
    public const float MAX_ROTATION_SPEED = 180;

    void Start () {
        _rigidbody = transform.GetComponent<Rigidbody2D>();
    }

    #region Movements
    public void Movement(Vector2 movement)
    {
        _rigidbody.AddForce(movement * MAX_MOVE_SPEED);
    }

    public void Rotation(Vector2 rotation)
    {
        float heading = Mathf.Atan2(rotation. x, rotation.y);
        if (rotation.magnitude > InputManager.AXIS_DEAD_ZONE)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg), MAX_ROTATION_SPEED * TimeManager.instance.time);
    }
    #endregion
}

