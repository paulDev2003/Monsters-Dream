using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlaceToSpawnClones : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();
    public List<GameObject> spawnedMonsters = new List<GameObject>();

    public void RunSkillCoroutine(Monster owner, PlaceToSpawnClones spawnsClones, float skillTime)
    {
        StartCoroutine(BackToNormality(owner,spawnsClones,spawnedMonsters,skillTime));
    }

    IEnumerator BackToNormality(Monster owner, PlaceToSpawnClones spawnsClones, List<GameObject> spawnedMonsters, float skillTime)
    {
        yield return new WaitForSeconds(skillTime);
        owner.gameObject.SetActive(true);
        owner.ownList.Add(owner.gameObject);
        foreach (var clon in spawnedMonsters)
        {
            owner.ownList.Remove(clon);
            clon.SetActive(false);
        }
        foreach (var monster in owner.oppositeList)
        {
            Monster scriptMonster = monster.GetComponent<Monster>();
            scriptMonster.ChooseTarget(owner.ownList);
        }
        Destroy(gameObject);
    }
}
