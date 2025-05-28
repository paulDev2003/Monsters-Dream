using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Separete Bodie", menuName = "Scriptable Objects/Skills/Separete Bodie")]
public class SKSepareteBodie : SkillSO
{
    public GameObject monsterToClone;
    public GameObject placeToSpawn;
    [HideInInspector] public List<GameObject> spawnedMonsters = new List<GameObject>();
    public float skillTime = 5f;
    public override void ShootSkill(Monster owner)
    {
        Debug.Log("Ejecutando ShootSkill para: " + owner.name);
        GameObject instantiatedSpawnPoints = Instantiate(placeToSpawn, owner.transform);
        PlaceToSpawnClones spawnsClones = instantiatedSpawnPoints.GetComponent<PlaceToSpawnClones>();
        owner.gameObject.SetActive(false);
        spawnsClones.spawnedMonsters.Clear();
        foreach (var spawn in spawnsClones.spawnPoints)
        {           
            GameObject clonedMonster = Instantiate(monsterToClone, spawn.position, Quaternion.identity);
            Monster scriptMonster = clonedMonster.GetComponent<Monster>();
            scriptMonster.notShowInterface = true;
            if (owner.enemie)
            {
                owner.gameManager.enemieList.Add(clonedMonster);
                scriptMonster.enemie = true;
            }
            else
            {
                owner.gameManager.friendsList.Add(clonedMonster);
                scriptMonster.enemie = false;
            }
            GameObject targetToChoose = owner.oppositeList[Random.Range(0, owner.oppositeList.Count)];
            scriptMonster.target = scriptMonster.ChooseTarget(targetToChoose);
            spawnsClones.spawnedMonsters.Add(clonedMonster);
        }
        owner.ownList.Remove(owner.gameObject);
        foreach (var monster in owner.oppositeList)
        {
            Monster monsterScript = monster.GetComponent<Monster>();
            if (monsterScript.target == owner)
            {
                Debug.Log("Es iguaaal");
                monsterScript.target = monsterScript.ChooseTarget(monsterScript.oppositeList);
            }
        }
        spawnsClones.transform.parent = null;
        spawnsClones.RunSkillCoroutine(owner, spawnsClones, skillTime);
    }

    
}
