using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour {

    public GameObject DroppablePrefab;

    [Header("Drops Timers")]
    public const float MAX_DROP_COOLDOWN = 3;
    public const float BOOST_DROP_RATE = 10f;
    float dropCurrentCooldown = 0;

    [Header("Drops Items")]
    public GameObject[] droppableItems;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        dropCurrentCooldown += TimeManager.instance.time;
        if(dropCurrentCooldown > BOOST_DROP_RATE)
        {
            Dropitem();
            dropCurrentCooldown = 0;
        }
    }

    void Dropitem()
    {
        GameObject droppedItem = Instantiate(DroppablePrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
