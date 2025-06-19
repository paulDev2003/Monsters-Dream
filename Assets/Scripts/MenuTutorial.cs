using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class MenuTutorial : MonoBehaviour
{
    public bool hasMadeTutorial = true;
    public GameObject prefabArrow;
    public MonstersHouse monstersHouse;
    public Inventory inventory;
    public List<Transform> spawnsPoints = new List<Transform>();
    public UnityEvent FirstEvent;
    public UnityEvent NurseryEvent;
    public UnityEvent ChooseSpotEgg;
    public UnityEvent OpenEggPanel;
    public UnityEvent UnlockedMonster;
    public UnityEvent LevelUpMonster;
    public UnityEvent FeedingMonster;
    public UnityEvent CloseFeed;
    public UnityEvent StartDungeon;
    public List<DiscoverMonster> monstersDiscovered = new List<DiscoverMonster>();
    public MonsterData firstMonster;
    public ItemSO itemToAdd;
    public ItemSO itemToAddTwo;
    public CurrentTeam currentTeam;
    public int totalAmountItem;
    private bool feeding;
    private int countFeed = 0;
    
    public GameObject arrowInstantiated;
    void Start()
    {
        int tutorial = PlayerPrefs.GetInt("TutorialMenu", 0);
        Debug.Log(tutorial);
        if (tutorial == 0)
        {
            hasMadeTutorial = false;
            Invoke("DelayFirstEvent", 1);
        }
    }

    public void DelayFirstEvent()
    {
        FirstEvent.Invoke();
    }

    public void SpawnArrow(int point)
    {
        arrowInstantiated = Instantiate(prefabArrow, spawnsPoints[point].position, prefabArrow.transform.rotation);
    }

    public void TutorialNursery()
    {
        if (!hasMadeTutorial)
        {
            NurseryEvent.Invoke();
        }
    }

    public void AddDiscoveredMonsters()
    {
        foreach (var discovered in monstersDiscovered)
        {
            monstersHouse.bestiary.Add(discovered);
        }
        List<ItemSO> itemsToLoot = new List<ItemSO>();
        for (int i = 0; i < totalAmountItem; i++)
        {
            itemsToLoot.Add(itemToAdd);
            if (itemToAddTwo != null)
            {
                itemsToLoot.Add(itemToAddTwo);
            }          
        }
        inventory.Additems(itemsToLoot);
        monstersHouse.listMonsters.Add(firstMonster);
    }

    public void ChooseSpotEggInvoke()
    {
        if (!hasMadeTutorial)
        {
            Destroy(arrowInstantiated);
            ChooseSpotEgg.Invoke();
        }        
    }

    public void OpenEggPanelCheck()
    {
        if (!hasMadeTutorial)
        {
            OpenEggPanel.Invoke();
        }
    }

    public void UnlockedMonsterCheck()
    {
        if (!hasMadeTutorial)
        {
            currentTeam.ActivateMonster(monstersHouse.listMonsters[1]);
            Destroy(arrowInstantiated);
            UnlockedMonster.Invoke();
        }
    }

    public void LevelUpMonsterChecked()
    {
        if (!hasMadeTutorial)
        {
            LevelUpMonster.Invoke();
        }
    }

    public void FeedingMonsterCheck()
    {
        if (!hasMadeTutorial)
        {
            feeding = true;
            FeedingMonster.Invoke();
        }
    }

    public void FeedToUp()
    {
        if (feeding)
        {
            if (countFeed < 1)
            {
                countFeed++;
            }
            else
            {
                CloseFeed.Invoke();
            }
        }
       
    }

    public void StartDungeonCheck()
    {
        if (!hasMadeTutorial)
        {
            Destroy(arrowInstantiated);
            StartDungeon.Invoke();
            PlayerPrefs.SetInt("TutorialMenu", 1);
            hasMadeTutorial = true;
            
        }
    }
}
