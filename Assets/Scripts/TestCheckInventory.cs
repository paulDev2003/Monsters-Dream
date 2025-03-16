using UnityEngine;

public class TestCheckInventory : MonoBehaviour
{
    private Inventory inventory;
    void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();
    }

    public void CheckInventory()
    {
        foreach (var item in inventory.capturableInventory)
        {
            string itemName = item.Key;  // Nombre del �tem
            ItemCapturable itemData = item.Value; // Datos del �tem
            int count = inventory.countCapturables.ContainsKey(itemName) ? inventory.countCapturables[itemName] : 0; // Cantidad del �tem

            Debug.Log($"Item: {itemName}, Cantidad: {count}");
        }
        foreach(var item in inventory.moleculeInventory)
        {
            string itemName = item.Key;  // Nombre del �tem
            ItemMolecule itemData = item.Value; // Datos del �tem
            int count = inventory.countMolecules.ContainsKey(itemName) ? inventory.countMolecules[itemName] : 0; // Cantidad del �tem

            Debug.Log($"Item: {itemName}, Cantidad: {count}");
        }
    }
}
