using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public GameObject prefab;
    public Sprite sprite;
}

public class Drop : MonoBehaviour
{

    public delegate void onItemDroped();
    public event onItemDroped OnItemDroped;

    public Item[] possibleItems;
    Item selectedItem;
    GameObject itemType;

    // Use this for initialization
    void Start ()
    {
        selectedItem = GetRandomItem();
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = selectedItem.sprite;
    }

    Item GetRandomItem()
    {
        return possibleItems[Random.Range(0, possibleItems.Length)];
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.CompareTag("Player"))
        {
            if (OnItemDroped != null)
                OnItemDroped();

            collider.transform.GetComponent<Player>().Drop(selectedItem.prefab);
            Destroy(gameObject);
        }
    }
}
