using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int maxCapacity;
    public List<Item> items = new List<Item>(); 
    
    public Item getItem(int index){
        return items[index];
    }

    public void removeItem(Item item)
    {
        items.Remove(item);
    }

    public void addItem(Item item){
        items.Add(item);
    }
}
