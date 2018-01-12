using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int playerID = 1;

    PlayerAbilities pAbilities;
    PlayerMovement pMovement;
    PlayerDamage pDamage;

    // Use this for initialization
    void Start () {
        pAbilities = transform.GetComponent<PlayerAbilities>();
        pMovement = transform.GetComponent<PlayerMovement>();
        pDamage = transform.GetComponent<PlayerDamage>();
    }
	
	// Update is called once per frame
	void Update () {
        Input();
	}

    public void Die()
    {

    }

    public void Drop(GameObject itemPrefab)
    {
        pAbilities.GetItem(itemPrefab);
    }

    void Input()
    {
        if (playerID == 2)
            return;

        Vector2 dirMoveThumbstick = InputManager.instance.GetThumstickAxis("Joy" + playerID + "LeftThumbstickX", "Joy" + playerID + "LeftThumbstickY");
        Vector2 rotMoveThumbstick = InputManager.instance.GetThumstickAxis("Joy" + playerID + "RightThumbstickX", "Joy" + playerID + "RightThumbstickY");
        pMovement.Movement(dirMoveThumbstick);
        pMovement.Rotation(rotMoveThumbstick);

        if (InputManager.instance.GetAxis("Joy" + playerID + "TriggerLeft") > 0)
            pAbilities.Dash(dirMoveThumbstick, InputManager.instance.GetAxis("Joy" + playerID + "TriggerLeft"));
        
        if (InputManager.instance.GetButton("Joy" + playerID + "BumperRight"))
            pAbilities.ShockWave();

        if (InputManager.instance.GetAxis("Joy" + playerID + "TriggerRight") > 0)
            pAbilities.UseItem();

        
    }

    public void Damage(Vector2 dir, float power)
    {
        pDamage.GetDamage(dir, power);
    }

}
