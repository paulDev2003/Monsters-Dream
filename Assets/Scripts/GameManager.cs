using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<GameObject> friendsList = new List<GameObject>();
    public List<GameObject> enemieList = new List<GameObject>();
    public GameObject finalScreen;
    public bool finish = false;
    public GameObject textInfoPrefab;
    public GameObject canvasWorld;

    public void RemoveFromList(List<GameObject> list, Monster monsterdead)
    {
        for (int i = list.Count - 1; i >= 0; i--) // recorre la lista en reversa
        {
            if (list[i] == monsterdead.gameObject)
            {
                list.RemoveAt(i);
                Debug.Log("Encuentra el igual");
            }
        }
        foreach (var monster in list)
        {
            Debug.Log(monster.name);
        }
    }

    public void CheckIfAnyAlive(List<GameObject> list)
    {
        foreach (var monster in list)
        {
            Monster monsterScript = monster.GetComponent<Monster>();
            if (!monsterScript.dead)
            {
                return;
            }
        }
        finalScreen.SetActive(true);
        finish = true;
    }
}
