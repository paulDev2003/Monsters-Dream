using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class GameDataController : MonoBehaviour
{
    public string saveFiles;
    public SaveData saveData = new SaveData();
    private Inventory inventory;

    private void Awake()
    {
        saveFiles = Application.dataPath + "/gameData.json";
    }

    private void Start()
    {
        inventory = FindAnyObjectByType<Inventory>(); // Se asegura de encontrarlo en Start()
        if (inventory == null)
        {
            Debug.Log("Can´t find inventory");
        }
    }

    public void LoadData(Inventory inventory)
    {
        if (File.Exists(saveFiles))
        {
            string content = File.ReadAllText(saveFiles);
            saveData = JsonUtility.FromJson<SaveData>(content);
            if (inventory == null)
            {
                Debug.Log("inventory es null");
            }
            if (saveData == null)
            {
                Debug.Log("Can´t find saveData");
            }
            //inventory.capturableInventory = new Dictionary<string, ItemCapturable>();
            //inventory.countCapturables = new Dictionary<string, int>();

            for (int i = 0; i < saveData.capturableKeys.Count; i++)
            {
                inventory.capturableInventory.Add(saveData.capturableKeys[i], saveData.capturableValues[i]);
                inventory.countCapturables.Add(saveData.capturableKeys[i], saveData.capturableAmount[i]);
            }

            //inventory.moleculeInventory = new Dictionary<string, ItemMolecule>();
            //inventory.countMolecules = new Dictionary<string, int>();

            for (int i = 0; i < saveData.molecularKeys.Count; i++)
            {
                inventory.moleculeInventory.Add(saveData.molecularKeys[i], saveData.molecularValues[i]);
                inventory.countMolecules.Add(saveData.molecularKeys[i], saveData.molecularAmount[i]);
            }
        }
        else
        {
            Debug.Log("The files doesn´t exist");
            if (inventory == null)
            {
                Debug.Log("Can´t find Inventory");
                return;
            }
            ResetInventory();
        }
    }

    public void SaveData()
    {
        SaveData newData = new SaveData()
        {
            capturableKeys = new List<string>(inventory.capturableInventory.Keys),
            capturableValues = new List<ItemCapturable>(inventory.capturableInventory.Values),
            capturableAmount = new List<int>(inventory.countCapturables.Values),

            molecularKeys = new List<string>(inventory.moleculeInventory.Keys),
            molecularValues = new List<ItemMolecule>(inventory.moleculeInventory.Values),
            molecularAmount = new List<int>(inventory.countMolecules.Values)
        };

        string chainJSON = JsonUtility.ToJson(newData);
        File.WriteAllText(saveFiles, chainJSON);
    }

    public void DeleteSaveData()
    {
        inventory.capturableInventory.Clear();
        inventory.countCapturables.Clear();
        inventory.moleculeInventory.Clear();
        inventory.countMolecules.Clear();
        SaveData();
    }

    public void ResetInventory()
    {
        inventory.capturableInventory = new Dictionary<string, ItemCapturable>();
        inventory.moleculeInventory = new Dictionary<string, ItemMolecule>();
        inventory.countMolecules = new Dictionary<string, int>();
        inventory.countCapturables = new Dictionary<string, int>();
    }
}
