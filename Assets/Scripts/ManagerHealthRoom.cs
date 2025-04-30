using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class ManagerHealthRoom : MonoBehaviour
{
    public List<GameObject> summonsUI = new List<GameObject>();
    public List<HealthObject> healthScripts = new List<HealthObject>();
    public MonsterDataBase monsterDataBase;
    public DungeonTeam dungeonTeam;
    public TextMeshProUGUI txtPercentage;
    public Image imgPercentage;
    public GameObject btnSkip;
    public GameObject btnNextRoom;
    private int maxPercentageHealth;
    private int percentageToHealth;


    private void Start()
    {
        maxPercentageHealth = PlayerPrefs.GetInt("Santuary", 50);
        txtPercentage.text = $"{maxPercentageHealth}%";
        percentageToHealth = maxPercentageHealth;
        int i = 0;
        foreach (var monster in dungeonTeam.allMonsters)
        {
            if (monster.monsterName != "")
            {
                summonsUI[i].SetActive(true);
                MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monster.monsterName);
                healthScripts[i].spriteMonster.sprite = monsterBase.monsterSO.sprite;
                MonsterClass monsterClass = new MonsterClass(monsterBase.monsterSO, monster.level);
                int maxHealth = monsterClass.Health;
                healthScripts[i].barContent.fillAmount = monster.currentHealth / (float)maxHealth;
                healthScripts[i].activated = true;
                i++;
            }
        }
    }

    public void PushButton()
    {
        StartCoroutine(ConvertPercentageToHealth());
    }

    private IEnumerator ConvertPercentageToHealth()
    {
        btnSkip.SetActive(false);
        btnNextRoom.SetActive(true);
        while (percentageToHealth > 0)
        {
            percentageToHealth--;
            txtPercentage.text = $"{percentageToHealth}%";
            imgPercentage.fillAmount = (float)percentageToHealth / (float)maxPercentageHealth;
            for (int i = 0; i < dungeonTeam.allMonsters.Count; i++)
            {
                var monster = dungeonTeam.allMonsters[i];
                if (monster.monsterName == "") continue;

                MonsterBase monsterBase = monsterDataBase.GetMonsterBaseByName(monster.monsterName);
                MonsterClass monsterClass = new MonsterClass(monsterBase.monsterSO, monster.level);
                int maxHealth = monsterClass.Health;

                float healthToAdd = maxHealth / 100f; // Cada 1% equivale a este valor
                monster.currentHealth = Mathf.Min(monster.currentHealth + healthToAdd, maxHealth);

                // Actualizar barra visual
                if (i < healthScripts.Count)
                    healthScripts[i].barContent.fillAmount = monster.currentHealth / (float)maxHealth;
            }

            yield return new WaitForSeconds(0.05f); // Controla la velocidad del efecto
        }
        percentageToHealth = 0;
        txtPercentage.text = $"{percentageToHealth}%";
    }
}
