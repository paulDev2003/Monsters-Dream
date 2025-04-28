using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class GameDataController : MonoBehaviour
{
    public string saveFiles;
    public SaveData saveData = new SaveData();
    public DataBaseSO itemDataBaseSO;
    public MonsterDataBase monsterDataBase;
    public RuneDataBase runeDataBase;
    private Inventory inventory;
    private MonstersHouse monstersHouse;
    private DungeonTeam dungeonTeam;
    private ManagerRunes runesManager;


    private void Awake()
    {
        saveFiles = Application.dataPath + "/gameData.json";
    }

    private void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();
        monstersHouse = FindAnyObjectByType<MonstersHouse>();
        dungeonTeam = FindAnyObjectByType<DungeonTeam>();
        runesManager = FindAnyObjectByType<ManagerRunes>();
        if (inventory == null)
        {
            Debug.Log("Can´t find inventory");
        }
    }

    public void LoadData(Inventory inventory, MonstersHouse monstersHouse, DungeonTeam dungeonTeam, ManagerRunes runesManager)
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
                var id = saveData.capturableIDs[i];
                var item = itemDataBaseSO.GetCapturableByID(id);
                if (item != null)
                {
                    inventory.capturableInventory.Add(saveData.capturableKeys[i], item);
                    inventory.countCapturables.Add(saveData.capturableKeys[i], saveData.capturableAmount[i]);
                }
            }

            for (int i = 0; i < saveData.molecularKeys.Count; i++)
            {
                var id = saveData.molecularIDs[i];
                var item = itemDataBaseSO.GetMoleculeByID(id);
                if (item != null)
                {
                    inventory.moleculeInventory.Add(saveData.molecularKeys[i], item);
                    inventory.countMolecules.Add(saveData.molecularKeys[i], saveData.molecularAmount[i]);
                }
            }

            if (saveData.monstersHouse != null)
            {
                monstersHouse.listMonsters = new List<MonsterData>(saveData.monstersHouse);
            }
            if (saveData.monstersDungeon != null)
            {
                dungeonTeam.allMonsters = new List<MonsterData>(saveData.monstersDungeon);
            }
            if (saveData.runesDungeon != null)
            {
                runesManager.runesDungeon = new List<RuneClass>(saveData.runesDungeon);
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
        List<RuneClass> listRunes = new List<RuneClass>();
        foreach (var rune in runesManager.allRunes)
        {
            RuneClass runeClass = new RuneClass()
            {
                runeName = rune.runeName,
                level = rune.level,
                savePosition = rune.savePosition,
                cost = rune.cost
            };
            listRunes.Add(runeClass);
        }
        SaveData newData = new SaveData()
        {
            capturableKeys = new List<string>(inventory.capturableInventory.Keys),
            capturableIDs = new List<string>(),
            capturableAmount = new List<int>(inventory.countCapturables.Values),

            molecularKeys = new List<string>(inventory.moleculeInventory.Keys),
            molecularIDs = new List<string>(),
            molecularAmount = new List<int>(inventory.countMolecules.Values),

            monstersHouse = new List<MonsterData>(monstersHouse.listMonsters),
            monstersDungeon = new List<MonsterData>(dungeonTeam.allMonsters),
            
            runesDungeon = new List<RuneClass>(listRunes)
        };
        foreach (var value in inventory.capturableInventory)
            newData.capturableIDs.Add(value.Key);

        foreach (var value in inventory.moleculeInventory)
            newData.molecularIDs.Add(value.Key);

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
