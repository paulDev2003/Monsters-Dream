using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public List<string> capturableKeys;
    public List<ItemCapturable> capturableValues;
    public List<int> capturableAmount;

    public List<string> molecularKeys;
    public List<ItemMolecule> molecularValues;
    public List<int> molecularAmount;
}
