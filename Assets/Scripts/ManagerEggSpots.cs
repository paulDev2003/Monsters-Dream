using UnityEngine;
using System.Collections.Generic;

public class ManagerEggSpots : MonoBehaviour
{
    public List<EggSpot> eggSpots = new List<EggSpot>();
    public MonstersHouse monstersHouse;
    public MonsterDataBase monsterDataBase;
    public Bestiary bestiary;

    private void Start()
    {
        foreach (var egg in monstersHouse.eggs)
        {
            MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(egg.monsterName);
            GameObject eggPrefab = monsterBase.monsterSO.egg;

            GameObject eggInstantiated = Instantiate(eggPrefab, eggSpots[egg.id].spawnEgg.position, eggPrefab.transform.rotation);
            Egg scriptEgg = eggInstantiated.GetComponent<Egg>();
            eggSpots[egg.id].progressBar.SetActive(true);
            eggSpots[egg.id].imgSuperiorBar.fillAmount = egg.currentPoints / (float)scriptEgg.eggSO.totalPoints;
            scriptEgg.growing = true;
            scriptEgg.bestiary = bestiary;
            scriptEgg.eggSpot = eggSpots[egg.id];
            scriptEgg.eggData = egg;
            if (egg.currentPoints == 0)
            {
                eggSpots[egg.id].imgSuperiorBar.fillAmount = 0.01f;
            }
        }
    }
}
