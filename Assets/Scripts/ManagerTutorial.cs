using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class ManagerTutorial : MonoBehaviour
{
    public UnityEvent BeginEvent;
    public UnityEvent FirstTutorial;
    public UnityEvent SecondTutorial;
    public UnityEvent SecondTutorialSelectEnemie;
    public UnityEvent ThirdTutorial;
    public UnityEvent ThirdTutorialTargeted;
    public UnityEvent EventVictory;
    public UnityEvent BackToGame;
    public GameManager gameManager;
    public GameObject monsterTwo;
    public GameObject monsterThree;
    public SkillDrop skillDropTargeted;
    public float delaySeconds = 2f;
    public bool firstPart = false;
    public bool secondPart = false;
    public bool thirdPart = false;
    private bool secondPartChoose = false;
    private Monster targetMonster;
    private Monster currentMonster;
    private bool showedArea = false;
    void Start()
    {
        BeginEvent.Invoke();
        Invoke("FirstTutorialDelay", delaySeconds);
    }

    private void Update()
    {
        if (secondPart && !thirdPart)
        {
            if (gameManager.monsterSelected != null && !secondPartChoose)
            {
                SecondTutorialSelectEnemie.Invoke();
                secondPartChoose = true;
                currentMonster = gameManager.monsterSelected.GetComponent<Monster>();
                targetMonster = currentMonster.target;
                foreach (var monster in gameManager.friendsList)
                {
                    monster.GetComponentInChildren<ClickOnMonster>().enabled = false;
                }
            }
            else if (secondPartChoose && targetMonster != currentMonster.target)
            {
                BackToTheGame();
            }
        }
        if (skillDropTargeted.showingArea)
        {
            showedArea = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (showedArea)
            {
                BackToGame.Invoke();
                Time.timeScale = 1;
                showedArea = false;
            }
        }
    }
    private void FirstTutorialDelay()
    {
        FirstTutorial.Invoke();
        Time.timeScale = 0;
    }

    public void BackToTheGame()
    {
        bool onButton = IsPointerOverButton();
        Debug.Log(onButton);
        if (!onButton)
        {
            BackToGame.Invoke();
            Time.timeScale = 1;
            firstPart = true;
        }        
    }

    public bool IsPointerOverButton()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // Si hay cualquier resultado de UI, significa que el puntero está sobre algo de la UI
        return results.Count > 0;
    }

    public void CheckPart()
    {
        if (secondPart == false)
        {
            SecondPart();
        }
        else if (thirdPart == false)
        {
            ThirdPart();
        }
        else
        {
            EventVictory.Invoke();
        }
    }

    public void SecondPart()
    {
        GameObject enemySpawned = Instantiate(monsterTwo, gameManager.enemySpawnPoints[1].position, Quaternion.identity);
        Monster enemyScript = enemySpawned.GetComponent<Monster>();
        Rigidbody rbEnemy = enemyScript.GetComponentInChildren<Rigidbody>();
        rbEnemy.isKinematic = true;
        enemyScript.level = Random.Range(1, 2);
        enemyScript.enemie = true;
        gameManager.enemieList.Add(enemySpawned);
        GameObject enemySpawnedThree = Instantiate(monsterThree, gameManager.enemySpawnPoints[2].position, Quaternion.identity);
        Monster enemyScriptThree = enemySpawnedThree.GetComponent<Monster>();
        Rigidbody rbEnemyThree = enemyScriptThree.GetComponentInChildren<Rigidbody>();
        rbEnemyThree.isKinematic = true;
        enemyScriptThree.level = Random.Range(1, 2);
        enemyScriptThree.enemie = true;
        gameManager.enemieList.Add(enemySpawnedThree);
        
        int i = 0;
        foreach (var enemie in gameManager.enemieList)
        {
            Monster scriptMonster = enemie.GetComponent<Monster>();
            if (scriptMonster.lifeBar == null)
            {
                gameManager.lifeBarsEnemies[i].SetActive(true);
                gameManager.lifeBarsEnemies[i].GetComponent<Image>().sprite = scriptMonster.monsterSO.sprite;
                scriptMonster.lifeBar = gameManager.superiorBarEnemies[i];
                scriptMonster.shieldBar = gameManager.shieldsEnemies[i];
                gameManager.levelEnemies[i].text = $"Lv.{scriptMonster.level}";
                i++;
            }
        }
        gameManager.finish = false;
        Invoke("SecondTutorialDelay", 3f);
    }

    private void SecondTutorialDelay()
    {
        SecondTutorial.Invoke();
        gameManager.monsterSelected = null;
        if (gameManager.selectorActive != null)
        {
            Destroy(gameManager.selectorActive);
        }
        secondPart = true;
        Time.timeScale = 0;
    }

    private void ThirdPart()
    {
        GameObject enemySpawned = Instantiate(monsterTwo, gameManager.enemySpawnPoints[0].position, Quaternion.identity);
        Monster enemyScript = enemySpawned.GetComponent<Monster>();
        Rigidbody rbEnemy = enemyScript.GetComponentInChildren<Rigidbody>();
        rbEnemy.isKinematic = true;
        enemyScript.level = Random.Range(1, 2);
        enemyScript.enemie = true;
        gameManager.enemieList.Add(enemySpawned);
        if (enemyScript.lifeBar == null)
        {
            gameManager.lifeBarsEnemies[0].SetActive(true);
            gameManager.lifeBarsEnemies[0].GetComponent<Image>().sprite = enemyScript.monsterSO.sprite;
            enemyScript.lifeBar = gameManager.superiorBarEnemies[0];
            enemyScript.shieldBar = gameManager.shieldsEnemies[0];
            gameManager.levelEnemies[0].text = $"Lv.{enemyScript.level}";
        }
        gameManager.finish = false;
        Invoke("ThirdTutorialDelay", 1.5f);
    }

    private void ThirdTutorialDelay()
    {
        ThirdTutorial.Invoke();
        Time.timeScale = 0;
        thirdPart = true;
    }

    public void TargetedSkill()
    {
        Time.timeScale = 1f;
        BackToGame.Invoke();
        Invoke("TargetedDelay", 1.5f);
    }

    public void TargetedDelay()
    {
        Time.timeScale = 0f;
        ThirdTutorialTargeted.Invoke();
    }

}
