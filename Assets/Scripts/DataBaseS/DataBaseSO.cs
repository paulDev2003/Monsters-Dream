using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DataBaseSO", menuName = "Scriptable Objects/DataBaseSO")]
public class DataBaseSO : ScriptableObject
{
    public List<ItemCapturable> capturableItems;
    public List<ItemMolecule> molecularItems;

    private Dictionary<string, ItemCapturable> capturableDict;
    private Dictionary<string, ItemMolecule> molecularDict;

    public void Initialize()
    {
        capturableDict = new Dictionary<string, ItemCapturable>();
        foreach (var item in capturableItems)
        {
            if (!capturableDict.ContainsKey(item.itemName))
                capturableDict[item.itemName] = item;
        }

        molecularDict = new Dictionary<string, ItemMolecule>();
        foreach (var item in molecularItems)
        {
            if (!molecularDict.ContainsKey(item.itemName))
                molecularDict[item.itemName] = item;
        }
    }

    public ItemCapturable GetCapturableByID(string id)
    {
        if (capturableDict == null) Initialize();
        return capturableDict.TryGetValue(id, out var item) ? item : null;
    }

    public ItemMolecule GetMoleculeByID(string id)
    {
        if (molecularDict == null) Initialize();
        return molecularDict.TryGetValue(id, out var item) ? item : null;
    }
}
