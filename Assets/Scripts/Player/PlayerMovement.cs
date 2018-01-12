using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D _rigidbody;
    private Player _player;

    [Header("Move")]
    public const float MAX_MOVE_SPEED = 50;
    
    void Start () {
        _rigidbody = transform.GetComponent<Rigidbody2D>();
        _player = transform.GetComponent<Player>();
    }

	void Update ()
    {
        if (_player.playerID == 2)
            return;
        Rotation();
        Movement();
    }

    #region Movements
    void Movement()
    {
        Vector2 movement = InputManager.instance.GetThumstickAxis("Joy" + _player.playerID + "LeftThumbstickX", "Joy" + _player.playerID + "LeftThumbstickY");
        _rigidbody.AddForce(movement * MAX_MOVE_SPEED);
    }

    void Rotation()
    {
        Vector2 rotation = InputManager.instance.GetThumstickAxis("Joy" + _player.playerID + "RightThumbstickX", "Joy" + _player.playerID + "RightThumbstickY");
        float heading = Mathf.Atan2(rotation. x, rotation.y);
        if (rotation.magnitude > InputManager.AXIS_DEAD_ZONE)
            transform.rotation = Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg);
    }
    #endregion
}

