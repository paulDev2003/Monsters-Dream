using UnityEngine;
using System.Collections.Generic;

public class TestListItems : MonoBehaviour
{
    private Inventory inventory;
    public List<ItemSO> loot;
    void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();
    }

    public void CallInventoryLoot()
    {
        inventory.Additems(loot);
    }

    
}
