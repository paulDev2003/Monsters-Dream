using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public Dictionary<string, ItemCapturable> capturableInventory;
    public Dictionary<string, ItemMolecule> moleculeInventory;

    public Dictionary<string, int> countMolecules;
    public Dictionary<string, int> countCapturables;
    private GameDataController gameDataController;

    public List<GameObject> monstersInventory = new List<GameObject>();
    public MonstersHouse monstersHouse;
    public DungeonTeam dungeonTeam;

    private void Start()
    {
        capturableInventory = new Dictionary<string, ItemCapturable>();
        moleculeInventory = new Dictionary<string, ItemMolecule>();
        countMolecules = new Dictionary<string, int>();
        countCapturables = new Dictionary<string, int>();
        gameDataController = FindAnyObjectByType<GameDataController>();
        if (gameDataController != null)
        {
            gameDataController.LoadData(this, monstersHouse, dungeonTeam);
        }
        else
        {
            Debug.Log("Can´t find DataController");
            
        }
    }
    public void Additems(List<ItemSO> listLoot)
    {
        foreach (ItemSO item in listLoot)
        {
            switch (item.type)
            {
                case ItemSO.typeItem.Capturable:
                    if (capturableInventory.ContainsKey(item.itemName))
                    {
                        countCapturables[item.itemName] += 1;
                    }
                    else
                    {
                        capturableInventory.Add(item.itemName, item as ItemCapturable);
                        countCapturables.Add(item.itemName, 1);                       
                    }
                    
                    break;
                case ItemSO.typeItem.Molecule:
                    if (moleculeInventory.ContainsKey(item.itemName))
                    {
                        countMolecules[item.itemName] += 1;
                    }
                    else
                    {
                        moleculeInventory.Add(item.itemName, item as ItemMolecule);
                        countMolecules.Add(item.itemName, 1);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
