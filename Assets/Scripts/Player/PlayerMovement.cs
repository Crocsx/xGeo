using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    Player _player;

    [Header("Move")]
    public float MAX_MOVE_SPEED = 125;

    [Header("Rotation")]
    public float MAX_ROTATION_SPEED = 80;

    void Start () {
        _player = transform.GetComponent<Player>();
        _player.OnResetPlayer += ResetMovement;
    }

    #region Movements
    public void Movement(Vector2 movement)
    {
        _player.pRigidbody.AddForce(movement * MAX_MOVE_SPEED);
    }

    public void Rotation(Vector2 rotation)
    {
        float heading = Mathf.Atan2(rotation. x, rotation.y);
        if (rotation.magnitude > InputManager.AXIS_DEAD_ZONE)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg), MAX_ROTATION_SPEED * TimeManager.instance.time);
    }
    #endregion

    #region Reset
    void ResetMovement()
    {
        MAX_MOVE_SPEED = 125;
        MAX_ROTATION_SPEED = 80;
    }
    #endregion
}

