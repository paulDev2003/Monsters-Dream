using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CurrentTeam : MonoBehaviour
{
    public MonstersHouse monstersHouse;
    public BoxSelectMonster monsterSelected;
    public BoxSelectMonster teamMonsterSelected;
    public MonsterDataBase monsterDataBase;
    public GameObject[] iconsList = new GameObject[6];
    public TextMeshProUGUI[] textsList = new TextMeshProUGUI[6];
    public List<GameObject> allIcons = new List<GameObject>();
    public List<TextMeshProUGUI> allTexts = new List<TextMeshProUGUI>();
    [System.NonSerialized]
    public List<MonsterData> allMonsters;
    public int i = 0;

    public void ActivateAllIcons()
    {
        int e = 0;
        foreach (var monster in monstersHouse.listMonsters)
        {
            MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monster.monsterName);
            Monster monsterScript = monsterBase.prefabMonster.GetComponent<Monster>();
            allIcons[e].SetActive(true);
            allIcons[e].GetComponent<Image>().sprite = monsterScript.monsterSO.sprite;
            allIcons[e].GetComponent<BoxSelectMonster>().monsterData = monster;
            allTexts[e].text = monster.monsterName;
            e++;
        }
    }
    public void AddMonster()
    {
        if (i > 5 || monsterSelected == null || monsterSelected.isUsed)
        {
            if (i>5)
            {
                Debug.Log("i es mayor a 5");
            }
            if (monsterSelected == null)
            {
                Debug.Log("Monster Selected es igual a null");
            }
            if (monsterSelected.isUsed)
            {
                Debug.Log("Monster Selected is used");
            }
            return;
        }
        MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monsterSelected.monsterData.monsterName);
        Monster monsterScript = monsterBase.prefabMonster.GetComponent<Monster>();
        iconsList[i].SetActive(true);
        iconsList[i].GetComponent<Image>().sprite = monsterScript.monsterSO.sprite;
        BoxSelectMonster boxSelect = iconsList[i].GetComponent<BoxSelectMonster>();
        boxSelect.monsterData = monsterSelected.monsterData;
        boxSelect.savedBox = monsterSelected;
        textsList[i].text = monsterSelected.monsterData.monsterName;
        monsterSelected.isUsed = true;
        monsterSelected.i = i;
        i++;
    }

    public void ChangeMonster()
    {
        if (monsterSelected.isUsed || monsterSelected == null || teamMonsterSelected == null)
        {
            Debug.Log("Return en Change");
            return;
        }
        MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monsterSelected.monsterData.monsterName);
        Monster monsterScript = monsterBase.prefabMonster.GetComponent<Monster>();
        iconsList[teamMonsterSelected.i].GetComponent<Image>().sprite = monsterScript.monsterSO.sprite;
        textsList[teamMonsterSelected.i].text = monsterSelected.monsterData.monsterName;
        monsterSelected.isUsed = true;

        BoxSelectMonster boxSelect = iconsList[teamMonsterSelected.i].GetComponent<BoxSelectMonster>();
        boxSelect.monsterData = monsterSelected.monsterData;
        monsterSelected.i = teamMonsterSelected.i;
        teamMonsterSelected.savedBox.isUsed = false;
        teamMonsterSelected.savedBox = monsterSelected;
    }

    public void DeleteMonster()
    {
        if (teamMonsterSelected == null)
        {
            Debug.Log("No hay monstruo del equipo seleccionado para eliminar.");
            return;
        }

        int indexToRemove = teamMonsterSelected.i;
        teamMonsterSelected.savedBox.isUsed = false;

        // 2. Desplazar todos los monstruos una posición atrás a partir del eliminado
        for (int j = indexToRemove; j < i - 1; j++)
        {
            // Copiar sprite y nombre al slot actual desde el siguiente
            Image currentImage = iconsList[j].GetComponent<Image>();
            Image nextImage = iconsList[j + 1].GetComponent<Image>();
            currentImage.sprite = nextImage.sprite;

            textsList[j].text = textsList[j + 1].text;

            // Copiar lógica del BoxSelectMonster
            BoxSelectMonster currentBox = iconsList[j].GetComponent<BoxSelectMonster>();
            BoxSelectMonster nextBox = iconsList[j + 1].GetComponent<BoxSelectMonster>();

            currentBox.monsterData = nextBox.monsterData;
            currentBox.savedBox = nextBox.savedBox;
            currentBox.i = j;
            if (nextBox.savedBox != null)
                nextBox.savedBox.i = j;
        }

        // 3. Ocultar la última box que quedó libre
        iconsList[i - 1].SetActive(false);
        textsList[i - 1].text = "";
        BoxSelectMonster lastBox = iconsList[i - 1].GetComponent<BoxSelectMonster>();
        lastBox.monsterData = null;
        lastBox.savedBox = null;

        // 4. Actualizar el índice
        i--;

        // 5. Limpiar selección
        teamMonsterSelected = null;
    }

    public void SaveMonstersInAllMonsters()
    {
        int e = 0;
        foreach (var box in iconsList)
        {
            MonsterData monsterData = box.GetComponent<BoxSelectMonster>().monsterData;
            allMonsters[e] = monsterData;
            e++;
        }
    }
}
