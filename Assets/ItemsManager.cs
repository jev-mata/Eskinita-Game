using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<Item> items;
    public GameObject itemPrefabs;
    public void onStart()
    {
        foreach (var itemInList in items)
        {
            for (int x = 0; x < transform.childCount; x++)
            {
                Destroy(transform.GetChild(x).gameObject);
            }
            GameObject item = Instantiate(itemPrefabs, transform);
            item.GetComponent<ItemController>().SetImage(itemInList.sprite);
        }
    }

    // Update is called once per frame

}
