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
            if (saveData.bestiary != null)
            {
                monstersHouse.bestiary = new List<DiscoverMonster>(saveData.bestiary);
            }
            if (saveData.eggs != null)
            {
                monstersHouse.eggs = new List<EggData>(saveData.eggs);
            }
            if (saveData.runesDungeon != null)
            {
                runesManager.runesDungeon = new List<RuneClass>(saveData.runesDungeon);
            }
            if (saveData.upgradesDungeon != null)
            {
                runesManager.upgradesDungeon = new List<UpgradeData>(saveData.upgradesDungeon);
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
        List<UpgradeData> listUpgrades = new List<UpgradeData>();
        foreach (var upgrade in runesManager.allUpgrades)
        {
            UpgradeData upgradeData = new UpgradeData()
            {
                upgradeName = upgrade.upgradeName,
                cost = upgrade.cost
            };
            listUpgrades.Add(upgradeData);
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
            bestiary = new List<DiscoverMonster>(monstersHouse.bestiary),
            eggs = new List<EggData>(monstersHouse.eggs),
            
            runesDungeon = new List<RuneClass>(listRunes),
            upgradesDungeon = new List<UpgradeData>(listUpgrades)
        };
        foreach (var value in inventory.capturableInventory)
            newData.capturableIDs.Add(value.Key);

        foreach (var value in inventory.moleculeInventory)
            newData.molecularIDs.Add(value.Key);

        string chainJSON = JsonUtility.ToJson(newData);
        File.WriteAllText(saveFiles, chainJSON);
    }

    public void ResetInventory()
    {
        inventory.capturableInventory = new Dictionary<string, ItemCapturable>();
        inventory.moleculeInventory = new Dictionary<string, ItemMolecule>();
        inventory.countMolecules = new Dictionary<string, int>();
        inventory.countCapturables = new Dictionary<string, int>();
    }

    public void NewGame()
    {
        // 1. Eliminar archivo de guardado si existe
        if (File.Exists(saveFiles))
        {
            File.Delete(saveFiles);
            Debug.Log("Archivo de guardado eliminado. Comenzando nueva partida.");
        }

        // 2. Resetear inventario y demás datos
        ResetInventory();

        monstersHouse.listMonsters = new List<MonsterData>();
        monstersHouse.bestiary = new List<DiscoverMonster>();
        monstersHouse.eggs = new List<EggData>();

        dungeonTeam.allMonsters = new List<MonsterData>();

        runesManager.runesDungeon = new List<RuneClass>();
        runesManager.upgradesDungeon = new List<UpgradeData>();

        // 3. Crear un nuevo SaveData vacío (o con datos iniciales si querés agregar algún ítem inicial)
        saveData = new SaveData
        {
            capturableKeys = new List<string>(),
            capturableIDs = new List<string>(),
            capturableAmount = new List<int>(),

            molecularKeys = new List<string>(),
            molecularIDs = new List<string>(),
            molecularAmount = new List<int>(),

            monstersHouse = new List<MonsterData>(),
            monstersDungeon = new List<MonsterData>(),
            bestiary = new List<DiscoverMonster>(),
            eggs = new List<EggData>(),

            runesDungeon = new List<RuneClass>(),
            upgradesDungeon = new List<UpgradeData>()
        };

        // 4. Guardar el nuevo estado inicial (opcional)
        SaveData();
    }
}
