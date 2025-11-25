using UnityEngine;
using System.Collections.Generic;

public class AddItems : MonoBehaviour
{
    public Inventory inventory;
    public List<ItemSO> itemsToAdd;

    public void ItemsToInventory()
    {
        inventory.Additems(itemsToAdd);
    }
}
