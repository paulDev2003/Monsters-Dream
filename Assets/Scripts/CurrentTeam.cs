using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CurrentTeam : MonoBehaviour
{
    public MonstersHouse monstersHouse;
    public MenuTutorial lobbyTutorial;
    public BoxSelectMonster monsterSelected;
    public BoxSelectMonster teamMonsterSelected;
    public MonsterDataBase monsterDataBase;
    public GameObject[] iconsList = new GameObject[6];
    public TextMeshProUGUI[] textsList = new TextMeshProUGUI[6];
    public List<Image> starterIcons = new List<Image>();
    public List<GameObject> allIcons = new List<GameObject>();
    public List<TextMeshProUGUI> allTexts = new List<TextMeshProUGUI>();
    public List<MonsterData> allMonsters;
    public DungeonTeam dungeonTeam;
    public Image btnRigthArrow;
    public Image btnLeftArrow;
    private int countStarters = 0;
    public int i = 0;
    public int page = 1;

    private void Start()
    {
        allMonsters = new List<MonsterData>(dungeonTeam.firstTeam);
        foreach (var monster in allMonsters)
        {
            if (monster.monsterName == "")
            {
                return;
            }
            if (monster.isStarter)
            {
                starterIcons[i].enabled = true;
                countStarters++;
            }
            MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monster.monsterName);
            

            
            Monster monsterScript = monsterBase.prefabMonster.GetComponent<Monster>();
            iconsList[i].SetActive(true);
            iconsList[i].GetComponent<Image>().sprite = monsterScript.monsterSO.sprite;
            BoxSelectMonster boxSelect = iconsList[i].GetComponent<BoxSelectMonster>();
            boxSelect.monsterData = monster;
            //boxSelect.savedBox = monsterSelected;
            textsList[i].text = monster.monsterName;
           // monsterSelected.i = i;
            i++;
        }
        int tutorial = PlayerPrefs.GetInt("TutorialMenu", 0);
        if (tutorial == 0)
        {
            
            allMonsters = new List<MonsterData>() { null, null, null, null, null, null };
            allMonsters[0] = lobbyTutorial.firstMonster;                  
            starterIcons[i].enabled = true;
            
            countStarters++;            
            MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(allMonsters[0].monsterName);
            Monster monsterScript = monsterBase.prefabMonster.GetComponent<Monster>();
            iconsList[i].SetActive(true);
            iconsList[i].GetComponent<Image>().sprite = monsterScript.monsterSO.sprite;
            BoxSelectMonster boxSelect = iconsList[i].GetComponent<BoxSelectMonster>();
            boxSelect.monsterData = allMonsters[0];
            boxSelect.savedBox = allIcons[0].GetComponent<BoxSelectMonster>();
            textsList[i].text = allMonsters[0].monsterName;
            i++;
        }
    }
    public void ActivateAllIcons()
    {
        int e = 0;
        foreach (var monster in monstersHouse.listMonsters)
        {
            if (e < page * 16 && e >= (page - 1) * 16)
            {
                MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monster.monsterName);
                Monster monsterScript = monsterBase.prefabMonster.GetComponent<Monster>();
                allIcons[e].SetActive(true);
                allIcons[e].GetComponent<Image>().sprite = monsterScript.monsterSO.sprite;
                allIcons[e].GetComponent<BoxSelectMonster>().monsterData = monster;
                allTexts[e].text = monster.monsterName;
                
            }
            else if (e > page * 16)
            {
                btnRigthArrow.enabled = true;
            }
            e++;
        }
    }
    public void AddMonster()
    {
        if (i > 5 || monsterSelected == null || monsterSelected.monsterData.isUsed)
        {
            if (i>5)
            {
                Debug.Log("i es mayor a 5");
            }
            if (monsterSelected == null)
            {
                Debug.Log("Monster Selected es igual a null");
            }
            if (monsterSelected.monsterData.isUsed)
            {
                Debug.Log("Monster Selected is used");
            }
            return;
        }
        if (countStarters == 0)
        {
            starterIcons[i].enabled = true;
            monsterSelected.monsterData.isStarter = true;
            countStarters++;
        }
        MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monsterSelected.monsterData.monsterName);
        Monster monsterScript = monsterBase.prefabMonster.GetComponent<Monster>();
        iconsList[i].SetActive(true);
        iconsList[i].GetComponent<Image>().sprite = monsterScript.monsterSO.sprite;
        BoxSelectMonster boxSelect = iconsList[i].GetComponent<BoxSelectMonster>();
        boxSelect.monsterData = monsterSelected.monsterData;
        boxSelect.savedBox = monsterSelected;
        boxSelect.isUsed = true;
        monsterSelected.isUsed = true;
        textsList[i].text = monsterSelected.monsterData.monsterName;
        monsterSelected.monsterData.isUsed = true;
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
        if (teamMonsterSelected.monsterData.isStarter)
        {
            starterIcons[teamMonsterSelected.i].enabled = false;
            countStarters--;
        }
        
        int indexToRemove = teamMonsterSelected.i;
        //teamMonsterSelected.savedBox.isUsed = false;
        foreach (var monster in allIcons)
        {
            MonsterData monsterData = monster.GetComponent<BoxSelectMonster>().monsterData;
            if (monsterData.monsterName == teamMonsterSelected.monsterData.monsterName)
            {
                monsterData.isUsed = false;
                monsterData.isStarter = false;
                break;
            }
        }
        teamMonsterSelected.monsterData.isUsed = false;

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
        if (i > 0 && countStarters == 0)
        {
            starterIcons[0].enabled = true;
            iconsList[0].GetComponent<BoxSelectMonster>().monsterData.isStarter = true;
            countStarters++;
        }
        // 5. Limpiar selección
        teamMonsterSelected = null;
    }

    public void SaveMonstersInAllMonsters()
    {
        int e = 0;
        List<MonsterData> monstersList = new List<MonsterData>();
        foreach (var icon in iconsList)
        {
            BoxSelectMonster box = icon.GetComponent<BoxSelectMonster>();
            monstersList.Add(box.monsterData);
        }
        
        dungeonTeam.allMonsters = monstersList;
    }

    public void SelectStarter()
    {
        if (teamMonsterSelected == null)
        {
            Debug.Log("No hay monstruo seleccionado");
            return;
        }
        if (countStarters >= 3)
        {
            Debug.Log("Son suficientes starters");
            return;
        }
        if (teamMonsterSelected.monsterData.isStarter)
        {
            Debug.Log("Este monstruo ya es Starter");
            return;
        }
        teamMonsterSelected.monsterData.isStarter = true;
        countStarters++;
        starterIcons[teamMonsterSelected.i].enabled = true;
    }

    public void DeselectStarter()
    {
        if (teamMonsterSelected == null)
        {
            Debug.Log("No hay monstruo seleccionado");
            return;
        }
        if (!teamMonsterSelected.monsterData.isStarter)
        {
            Debug.Log("Este monstruo no es starter");
            return;
        }
        teamMonsterSelected.monsterData.isStarter = false;
        countStarters--;
        starterIcons[teamMonsterSelected.i].enabled = false;
        if (i > 0 && countStarters == 0)
        {
            starterIcons[0].enabled = true;
            iconsList[0].GetComponent<BoxSelectMonster>().monsterData.isStarter = true;
            countStarters++;
        }
    }

    public void LeftArrow()
    {
        int i = 0;
        btnRigthArrow.enabled = true;
        btnLeftArrow.enabled = false;
        page -= 1;
        foreach (var monster in monstersHouse.listMonsters)
        {
            if (i < (page - 1) * 16)
            {
                btnLeftArrow.enabled = true;
            }
            if (i < page * 16 && i >= (page - 1) * 16)
            {
                MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monster.monsterName);
                Monster monsterScript = monsterBase.prefabMonster.GetComponent<Monster>();
                allIcons[i - (page - 1) * 16].SetActive(true);
                allIcons[i - (page - 1) * 16].GetComponent<Image>().sprite = monsterScript.monsterSO.sprite;
                allIcons[i - (page - 1) * 16].GetComponent<BoxSelectMonster>().monsterData = monster;
                allTexts[i - (page - 1) * 16].text = monster.monsterName;
            }
            i++;
        }
    }
    private void CleanAll()
    {
        int e = 0;
        foreach (var icon in allIcons)
        {
            icon.SetActive(false);
            allTexts[e].text = "";
        }
    }
    public void RigthArrow()
    {
        CleanAll();
        int i = 0;
        btnRigthArrow.enabled = false;
        btnLeftArrow.enabled = true;
        page += 1;
        foreach (var monster in monstersHouse.listMonsters)
        {
            if (i < page * 16 && i >= (page - 1) * 16)
            {
                MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monster.monsterName);
                Monster monsterScript = monsterBase.prefabMonster.GetComponent<Monster>();
                allIcons[i - (page - 1) * 16].SetActive(true);
                allIcons[i - (page - 1) * 16].GetComponent<Image>().sprite = monsterScript.monsterSO.sprite;
                allIcons[i - (page - 1) * 16].GetComponent<BoxSelectMonster>().monsterData = monster;
                allTexts[i - (page - 1) * 16].text = monster.monsterName;
            }
            if (i > page * 16)
            {
                btnRigthArrow.enabled = true;
            }
            i++;
        }
    }

    public void ActivateMonster(MonsterData monsterDataAdd)
    {
        allMonsters[i] = monsterDataAdd;

        MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(allMonsters[i].monsterName);
        Monster monsterScript = monsterBase.prefabMonster.GetComponent<Monster>();
        iconsList[i].SetActive(true);
        iconsList[i].GetComponent<Image>().sprite = monsterScript.monsterSO.sprite;
        BoxSelectMonster boxSelect = iconsList[i].GetComponent<BoxSelectMonster>();
        boxSelect.monsterData = allMonsters[i];
        boxSelect.savedBox = allIcons[i].GetComponent<BoxSelectMonster>();
        textsList[i].text = allMonsters[i].monsterName;
        allMonsters[i].isStarter = false;
        i++;
    }
}
