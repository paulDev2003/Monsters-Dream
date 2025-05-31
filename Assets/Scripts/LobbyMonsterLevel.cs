using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class LobbyMonsterLevel : MonoBehaviour
{
    public List<Image> monstersImg = new List<Image>();
    public MonstersHouse monstersHouse;
    public MonsterDataBase monsterDataBase;
    public int pageNumber = 1;
    public UnityEvent EnabledInterface;
    public Transform spawnRender;
    private GameObject monsterPrefab;
    private bool insideCollider = false;
    private bool monsterSpawned = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && insideCollider)
        {
            
            EnabledInterface.Invoke();
        }
    }
    public void FillImages()
    {
        int i = 0;
        foreach (var monster in monstersHouse.listMonsters)
        {
            if (i < pageNumber * 9 && i >= (pageNumber - 1) * 9)
            {               
                monstersImg[i].enabled = true;
                MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monster.monsterName);
                monstersImg[i].sprite = monsterBase.monsterSO.sprite;
                monstersImg[i].GetComponent<MonsterSlot>().monsterName = monsterBase.monsterName;
                if (!monsterSpawned)
                {
                    monsterPrefab = Instantiate(monsterBase.prefabMonster, spawnRender.position, spawnRender.transform.rotation);
                    InstantiateMonsterRender();
                }
            }
            i++;
        }
    }

    public void ChangeMonsterPrefab(string monsterName)
    {
        MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monsterName);
        Destroy(monsterPrefab);
        monsterPrefab = Instantiate(monsterBase.prefabMonster, spawnRender.position, spawnRender.transform.rotation);
        InstantiateMonsterRender();
    }

    private void InstantiateMonsterRender()
    {
        monsterSpawned = true;
        monsterPrefab.GetComponent<Monster>().enabled = false;
        Rigidbody rb = monsterPrefab.GetComponentInChildren<Rigidbody>();
        rb.useGravity = false;
        Transform firstChild = monsterPrefab.transform.GetChild(0);
        firstChild.gameObject.AddComponent<CharacterPreviewRotation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            insideCollider = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            insideCollider = false;
        }
    }
}
