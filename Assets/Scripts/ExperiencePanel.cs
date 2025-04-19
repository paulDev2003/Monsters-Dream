using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ExperiencePanel : MonoBehaviour
{
    public Image sprite;
    public TextMeshProUGUI txtLevel;
    public TextMeshProUGUI txtName;
    public Image expBar;
    public Image backBar;
    public float animationSpeed = 1f; // Velocidad de animación
    private bool animating;
    private GameManager gameManager;

    private float targetFill;
    private float expGained;
    private MonsterData newDataMonster = new MonsterData();
    private Monster monsterScript;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    public MonsterData ShowPanel(MonsterSO monsterSO, int gainedExp, MonsterData monsterData)
    {
        newDataMonster.currentXP = monsterData.currentXP;
        newDataMonster.level = monsterData.level;
        expGained = gainedExp;
        sprite.enabled = true;
        sprite.sprite = monsterSO.sprite;
        txtLevel.enabled = true;
        txtLevel.text = $"Lv.{monsterData.level}";
        txtName.enabled = true;
        txtName.text = monsterData.monsterName;
        expBar.enabled = true;
        expBar.fillAmount = (float)monsterData.currentXP /((float)monsterData.level * 50f); ;
        float rest = (float)monsterData.currentXP + gainedExp;
        while (monsterData.currentXP + gainedExp >= newDataMonster.level * 50)
        {
            rest = newDataMonster.currentXP + gainedExp - newDataMonster.level * 50;
            newDataMonster.level += 1;
            txtLevel.text = $"Lv.{monsterData.level}";
        }
        newDataMonster.currentXP = (int)rest;
        Debug.Log((float)monsterData.currentXP);
        Debug.Log($"La experiencia en showPanel porcentual es: {expBar.fillAmount}");
        backBar.enabled = true;
        UpdateExperience(monsterData.currentXP, monsterData.level * 50, gainedExp, monsterData);
        return newDataMonster;
    }

    public void UpdateExperience(float currentXP, float maxXP, float gainedXP,  MonsterData originalData)
    {
        StartCoroutine(AnimateXPBar(currentXP, maxXP, gainedXP,originalData));
    }

    private IEnumerator AnimateXPBar(float currentXP, float maxXP, float gainedXP,  MonsterData dataMonster)
    {
        animating = true;
        float startFill = currentXP / maxXP;
        targetFill = (currentXP + gainedXP) / maxXP;
        float elapsedTime = 0f;
        float rest = currentXP + gainedXP;
        int savedLevel = dataMonster.level;
        while (elapsedTime < animationSpeed)
        {
            if (expBar.fillAmount >= 1)
            {
                rest = currentXP + gainedXP- maxXP;
                savedLevel += 1;
                maxXP = dataMonster.level *50;
                startFill = 0;
                txtLevel.text = $"Lv.{savedLevel}";
                
                targetFill = rest / maxXP;
            }
            elapsedTime += Time.deltaTime;
            
            expBar.fillAmount = Mathf.Lerp(startFill, targetFill, elapsedTime / animationSpeed);
            yield return null;
        }
        expBar.fillAmount = targetFill;
        gameManager.expCompleted = true;
    }

    public void FastUpdate(float gainedXP, MonsterData monsterData)
    {
        StopAllCoroutines();
        float startFill = monsterData.currentXP / (monsterData.level * 50);
        targetFill = (monsterData.currentXP + gainedXP) / (monsterData.level * 50);
        float rest = monsterData.currentXP + gainedXP;
        while (targetFill >= 1)
        {
            rest = monsterData.currentXP + gainedXP - monsterData.level * 50;
            monsterData.level += 1;
            startFill = 0;
            txtLevel.text = $"Lv.{monsterData.level}";
            targetFill = rest / (monsterData.level * 50);
            Debug.Log("Entra al while");
        }
        expBar.fillAmount = targetFill;
        animating = false;
        gameManager.expCompleted = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (animating)
            {
                FastUpdate(expGained, newDataMonster);
                animating = false;
            }
        }
    }
}
