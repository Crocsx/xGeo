using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public GameObject prefab;
    public Sprite sprite;
    public float ratio;
}

public class Drop : MonoBehaviour {


    public Item[] possibleItems;
    Item selectedItem;
    SpriteRenderer spriteRenderer;
    GameObject itemType;

    // Use this for initialization
    void Start () {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        selectedItem = GetRandomItem();
        spriteRenderer.sprite = selectedItem.sprite;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    Item GetRandomItem()
    {
        return possibleItems[0];
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.CompareTag("Player"))
        {
            collider.transform.GetComponent<Player>().Drop(selectedItem.prefab);
        }
        Destroy(gameObject);
    }
}
