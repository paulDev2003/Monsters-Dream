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
    private Monster monsterScript;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    public void ShowPanel(Monster monster, int gainedExp)
    {
        expGained = gainedExp;
        monsterScript = monster;
        sprite.enabled = true;
        sprite.sprite = monster.monsterSO.sprite;
        txtLevel.enabled = true;
        txtLevel.text = $"Lv.{monster.level}";
        txtName.enabled = true;
        txtName.text = monster.monsterName;
        expBar.enabled = true;
        expBar.fillAmount = (float)monster.exp / (float)monster.maxExp;
        backBar.enabled = true;
        UpdateExperience(monster.exp, monster.maxExp, gainedExp, monster);
    }

    public void UpdateExperience(float currentXP, float maxXP, float gainedXP, Monster monster)
    {
        StartCoroutine(AnimateXPBar(currentXP, maxXP, gainedXP, monster));
    }

    private IEnumerator AnimateXPBar(float currentXP, float maxXP, float gainedXP, Monster monster)
    {
        animating = true;
        float startFill = currentXP / maxXP;
        targetFill = (currentXP + gainedXP) / maxXP;
        float elapsedTime = 0f;
        float rest = 0;
        int lvlSaver = monster.level;
        while (elapsedTime < animationSpeed)
        {
            if (expBar.fillAmount >= 1)
            {
                rest = currentXP + gainedXP- maxXP;
                maxXP += 25;
                startFill = 0;
                lvlSaver += 1; //Esto se cambiará en el futuro
                txtLevel.text = $"Lv.{lvlSaver}";
                
                targetFill = rest / maxXP;
            }
            elapsedTime += Time.deltaTime;
            
            expBar.fillAmount = Mathf.Lerp(startFill, targetFill, elapsedTime / animationSpeed);
            yield return null;
        }
        expBar.fillAmount = targetFill;
        gameManager.expCompleted = true;
    }

    public void FastUpdate(float gainedXP, Monster monster)
    {
        StopAllCoroutines();
        float startFill = monster.exp / monster.maxExp;
        targetFill = (monster.exp + gainedXP) / monster.maxExp;
        float rest;
        int lvlSaver = monster.level;
        while (targetFill >= 1)
        {
            rest = monster.exp + gainedXP - monster.maxExp;
            monster.maxExp += 25;
            startFill = 0;
            lvlSaver += 1; 
            txtLevel.text = $"Lv.{lvlSaver}";
            targetFill = rest/ monster.maxExp;
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
                FastUpdate(expGained, monsterScript);
                animating = false;
            }
        }
    }
}
