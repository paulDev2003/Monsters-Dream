using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class MenuTutorial : MonoBehaviour
{
    bool hasMadeTutorial = true;
    public GameObject prefabArrow;
    public MonstersHouse monstersHouse;
    public Inventory inventory;
    public List<Transform> spawnsPoints = new List<Transform>();
    public UnityEvent FirstEvent;
    public UnityEvent NurseryEvent;
    public List<DiscoverMonster> monstersDiscovered = new List<DiscoverMonster>();
    void Start()
    {
        PlayerPrefs.SetInt("TutorialMenu", 1);
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
        GameObject arrow = Instantiate(prefabArrow, spawnsPoints[point].position, prefabArrow.transform.rotation);
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
    }
}
