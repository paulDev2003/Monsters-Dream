using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class MonsterDrop : MonoBehaviour
{
    public int valueI;
    public float timeCooldown;
    public float currentTimeCooldown;
    public bool available = true;
    public GameObject monsterSaved;
   [SerializeField] private bool isMonsterSelected = false; // Bandera para saber si ya se eligi� un monstruo
    public bool isUsed = false;
    public Monster monsterScript;
    private GameManager gameManager;
    private ManagerRunes runeManager;
    public GameObject instantiatedMonster;
    private GameObject markInstance;
    private GameObject saveMonsterMarked;
    private float healthReserve;
    public bool wasChanged = false;
    public bool areaInstantiated = false;
    private GameObject areaInstantiatedObj;
    private bool wasDown = false;
    private bool markInstantiated = false;
    public MonsterData monsterData;
    public Image imgCooldown;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        runeManager = FindAnyObjectByType<ManagerRunes>();
        if (monsterSaved !=null)
        {
            imgCooldown.enabled = true;
            if (!isUsed)
            {
                monsterScript = monsterSaved.GetComponent<Monster>();
            }
            
        }
        CheckUse();
    }

    void Update()
    {
        if (currentTimeCooldown > 0)
        {
            currentTimeCooldown -= Time.deltaTime;
            imgCooldown.fillAmount = currentTimeCooldown / timeCooldown;
            available = false;
        }
        else
        {
            available = true;
        }

        if (isMonsterSelected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (available && gameManager.countMonsters < 3)
                {
                    RangeArea();
                    wasDown = true;
                }                
            }           
            else if (Input.GetMouseButtonUp(0))
            {
                if (wasDown)
                {
                    SpawnMonster();
                    wasDown = false;
                }
                
            }
            if (wasDown)
            {
                RangeArea();
            }
        }
        
   
    }

    // M�todo para seleccionar el monstruo al hacer clic en la imagen
    public void SelectMonster()
    {
        //M�todo para que el summon vuelva al inventario y deje la pelea
        bool backNow = false;
        if (isUsed && instantiatedMonster == gameManager.monsterSelected)
        {
            Debug.Log("ES IGUAAL");
            if (gameManager.countMonsters > 1)
            {
                instantiatedMonster.SetActive(false);
                wasChanged = true;
                isUsed = false;
                backNow = true;
                gameManager.countMonsters--;
                gameManager.attacksPanel[monsterScript.valueI].targetImage.enabled = false;
                gameManager.friendsList.Remove(gameManager.monsterSelected);
                gameManager.monsterSelected = null;               
                ResetCooldown(monsterScript.valueI);
                Destroy(gameManager.selectorActive.gameObject);
                foreach (var enemie in gameManager.enemieList)
                {
                    Monster scriptEnemie = enemie.GetComponent<Monster>();
                    if (scriptEnemie == null)
                    {
                        scriptEnemie = enemie.GetComponentInChildren<Monster>();
                    }
                    if (scriptEnemie.target == monsterScript)
                    {

                        Debug.Log("cambia el target");                       
                        gameManager.friendsList.Remove(gameManager.monsterSelected);

                        
                        
                        scriptEnemie.target = scriptEnemie.ChooseTarget(scriptEnemie.oppositeList);
                        
                    }
                }
            }
        }
        if (isUsed || monsterScript.dead || backNow)
        {
            return;
        }
        if (gameManager.monsterSelected == null)
        {
            Debug.Log("Entra en SelectMonster");
            isMonsterSelected = true;
        }
        else
        {
            if (!available)
            {
                return;
            }
            Debug.Log("Hasta ac� bien");
            foreach (var monster in gameManager.monsterDrop)
            {
                if (monster.instantiatedMonster == gameManager.monsterSelected)
                {
                    Debug.Log("No entra nunca");
                    monster.isUsed = false;
                    monster.healthReserve = monster.monsterScript.healthFigth;
                    monster.wasChanged = true;
                    monsterScript.lifeBar = gameManager.superiorBarFriends[valueI];
                    monsterScript.shieldBar = gameManager.shieldsFriends[valueI];
                    gameManager.attacksPanel[monster.monsterScript.valueI].targetImage.sprite = monsterScript.monsterSO.skill.sprite;
                    monsterScript.valueI = monster.monsterScript.valueI;
                    monsterScript.skillDrop = gameManager.attacksPanel[monsterScript.valueI];
                    gameManager.attacksPanel[monsterScript.valueI].monsterOwner = monsterScript;
                    monster.ResetCooldown(monster.monsterScript.valueI);
                    if (wasChanged)
                    {
                        instantiatedMonster.SetActive(true);
                        instantiatedMonster.transform.position = monster.instantiatedMonster.transform.position;
                        instantiatedMonster.transform.rotation = monster.instantiatedMonster.transform.rotation;
                        Monster scriptIns = instantiatedMonster.GetComponent<Monster>();
                        scriptIns.target = monster.monsterScript.target;
                    }
                    else
                    {
                        if (monster.instantiatedMonster != null)
                        {
                            instantiatedMonster = Instantiate(monsterSaved, monster.instantiatedMonster.transform.position + Vector3.up * 2, monster.transform.rotation);
                            Monster scriptMonster = instantiatedMonster.GetComponent<Monster>();
                            runeManager.AddBuffs(scriptMonster);
                        }                       
                    }              
                    isMonsterSelected = false; // Resetear para evitar m�s instancias sin nueva selecci�n
                    isUsed = true;
                    gameManager.friendsList.Add(instantiatedMonster);
                    gameManager.friendsList.Remove(gameManager.monsterSelected);
                    Destroy(gameManager.selectorActive.gameObject);
                    monster.instantiatedMonster.SetActive(false);
                    gameManager.monsterSelected = null;
                    Monster scriptInstantiated = instantiatedMonster.GetComponent<Monster>();
                    monsterScript = scriptInstantiated;
                  //  monsterScript.valueI = monster.monsterScript.valueI;
                  //  monsterScript.UpdateBar();
                 //   monsterScript.lifeBar.UpdateFill(monsterScript);

                    Debug.Log(monster.monsterScript);
                    foreach (var enemie in gameManager.enemieList)
                    {
                        Debug.Log("Encuentra al igual");
                        Monster scriptEnemie = enemie.GetComponent<Monster>();
                        if (scriptEnemie.target == monster.monsterScript)
                        {

                            Debug.Log("cambia el target");
                            scriptEnemie.target = scriptInstantiated;
                        }
                    }
                    return;
                }
            }
        }
    }

    void SpawnMonster()
    {
        saveMonsterMarked = null;
        bool isOnButton = IsPointerOverButton();
        if (isOnButton)
        {
            Debug.Log("Is Pointer over Button");
            isMonsterSelected = false;
            areaInstantiatedObj.SetActive(false);
            return;
        }
        // Obtener la posici�n del clic en el mundo
        if (markInstance != null)
        {
            markInstance.SetActive(false);
        }
        areaInstantiatedObj.SetActive(false);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        isMonsterSelected = false; // Resetear para evitar m�s instancias sin nueva selecci�n
        if (gameManager.countMonsters < 3)
        {
            gameManager.countMonsters++;
            
            if (Physics.Raycast(ray, out hit)) // Para juegos 3D
            {
                if (!wasChanged)
                {
                    instantiatedMonster = Instantiate(monsterSaved, hit.point + Vector3.up * 2, Quaternion.identity);
                    Monster scriptSummon = instantiatedMonster.GetComponent<Monster>();
                    scriptSummon.exp = scriptSummon.monsterData.currentXP;
                    scriptSummon.level = scriptSummon.monsterData.level;
                    
                }
                else
                {
                    instantiatedMonster.SetActive(true);
                    instantiatedMonster.transform.position = hit.point + Vector3.up * 2;
                }
                isUsed = true;
                gameManager.friendsList.Add(instantiatedMonster);
                Monster scriptMonster = instantiatedMonster.GetComponent<Monster>();
                scriptMonster.lifeBar = gameManager.superiorBarFriends[valueI];
                scriptMonster.shieldBar = gameManager.shieldsFriends[valueI];
                runeManager.AddBuffs(scriptMonster);
                foreach (var attack in gameManager.attacksPanel)
                {
                    if (attack.targetImage.enabled == false)
                    {
                        attack.targetImage.enabled = true;
                        attack.targetImage.sprite = scriptMonster.monsterSO.skill.sprite;
                        attack.monsterOwner = scriptMonster;
                        scriptMonster.skillDrop = attack;
                        return;
                    }
                }
            }
            
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

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<Button>() != null) // Solo detecta botones
            {
                return true;
            }
        }
        return false;
    }


    void CheckUse()
    {
        if (monsterSaved != null)
        {
            if (isUsed)
            {
                isUsed = true;
                monsterScript = instantiatedMonster.GetComponent<Monster>();
            }
        }

    }

    private void RangeArea()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        float y = gameManager.damageArea.transform.position.y;
        Vector3 mouseWithHeigth = new Vector3(mousePosition.x, y, mousePosition.z);
        if (!areaInstantiated)
        {
            areaInstantiatedObj = Instantiate(gameManager.damageArea, mouseWithHeigth, Quaternion.identity);
            areaInstantiatedObj.transform.localScale = Vector3.one * monsterScript.distanceAttack * 0.25f;
            areaInstantiated = true;
        }
        if (areaInstantiatedObj.activeSelf == false)
        {
            areaInstantiatedObj.SetActive(true);
        }
        areaInstantiatedObj.transform.position = mouseWithHeigth;
        if (!gameManager.finish)
        {
            LookingForClosestEnemie(mousePosition);
        }       
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private void LookingForClosestEnemie(Vector3 mousePosition)
    {
        if (gameManager.finish)
        {
            return;
        }
        float closestDistance = Vector3.Distance(gameManager.enemieList[0].transform.position, mousePosition);
        GameObject monsterSelected = gameManager.enemieList[0];
        foreach (var monster in gameManager.enemieList)
        {
            float actualDistance = Vector3.Distance(monster.transform.position, mousePosition);
            if (closestDistance > actualDistance)
            {
                closestDistance = actualDistance;
                monsterSelected = monster;
            }
        }
        if (!markInstantiated)
        {
            markInstance = Instantiate(gameManager.closestMark, monsterSelected.transform.position + Vector3.up * 2, Quaternion.identity,
                monsterSelected.transform);
            markInstantiated = true;
        }
        else
        {
            markInstance.SetActive(true);
            if (saveMonsterMarked != monsterSelected)
            {
                markInstance.transform.position = monsterSelected.transform.position + Vector3.up * 2;
                markInstance.transform.parent = monsterSelected.transform;
            }                
        }
        saveMonsterMarked = monsterSelected;
    }

    private void ResetCooldown(int i)
    {
        available = false;
        imgCooldown.fillAmount = 1;
        currentTimeCooldown = timeCooldown;
        gameManager.attacksPanel[i].cooldownImage.fillAmount = 0;
    }
}
