using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public Dictionary<string, ItemCapturable> capturableInventory;
    public Dictionary<string, ItemMolecule> moleculeInventory;

    public Dictionary<string, int> countMolecules;
    public Dictionary<string, int> countCapturables;
    private GameDataController gameDataController;

    private void Start()
    {
        gameDataController = FindAnyObjectByType<GameDataController>();
        if (gameDataController != null)
        {
            gameDataController.LoadData();
        }
        else
        {
            Debug.Log("Can´t find DataController");
            capturableInventory = new Dictionary<string, ItemCapturable>();
            moleculeInventory = new Dictionary<string, ItemMolecule>();
            countMolecules = new Dictionary<string, int>();
            countCapturables = new Dictionary<string, int>();
        }
    }
    public void Additems(List<ItemSO> listLoot)
    {
        foreach (ItemSO item in listLoot)
        {
            switch (item.type)
            {
                case ItemSO.typeItem.Capturable:
                    if (capturableInventory.ContainsKey(item.name))
                    {
                        countCapturables[item.name] += 1;
                    }
                    else
                    {
                        capturableInventory.Add(item.name, item as ItemCapturable);
                        countCapturables.Add(item.name, 1);                       
                    }
                    
                    break;
                case ItemSO.typeItem.Molecule:
                    if (moleculeInventory.ContainsKey(item.name))
                    {
                        countMolecules[item.name] += 1;
                    }
                    else
                    {
                        moleculeInventory.Add(item.name, item as ItemMolecule);
                        countMolecules.Add(item.name, 1);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
