using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    xPlayer _player;

    [Header("Move")]
    public float MAX_MOVE_SPEED = 125;

    [Header("Rotation")]
    public float MAX_ROTATION_SPEED = 80;

    void Start ()
    {
        _player = transform.GetComponent<xPlayer>();
        _player.OnResetPlayer += ResetMovement;
    }

    #region Movements
    public void Movement(Vector2 movement)
    {
        _player.pRigidbody.AddForce(movement * MAX_MOVE_SPEED * TimeManager.instance.time * 100);
    }

    public void Rotation(Vector2 rotation)
    {
        if (rotation.magnitude < 0.25f)
            return;

        float heading = Mathf.Atan2(rotation. x, rotation.y);
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

    private void OnDestroy()
    {
        _player.OnResetPlayer -= ResetMovement;
    }
}

