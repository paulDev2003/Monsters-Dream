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
            string itemName = item.Key;  // Nombre del ítem
            ItemCapturable itemData = item.Value; // Datos del ítem
            int count = inventory.countCapturables.ContainsKey(itemName) ? inventory.countCapturables[itemName] : 0; // Cantidad del ítem

            Debug.Log($"Item: {itemName}, Cantidad: {count}");
        }
        foreach(var item in inventory.moleculeInventory)
        {
            string itemName = item.Key;  // Nombre del ítem
            ItemMolecule itemData = item.Value; // Datos del ítem
            int count = inventory.countMolecules.ContainsKey(itemName) ? inventory.countMolecules[itemName] : 0; // Cantidad del ítem

            Debug.Log($"Item: {itemName}, Cantidad: {count}");
        }
    }
}
