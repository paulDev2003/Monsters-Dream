using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class MonsterDrop : MonoBehaviour
{
    public GameObject monsterSaved;
   [SerializeField] private bool isMonsterSelected = false; // Bandera para saber si ya se eligió un monstruo
    public bool isUsed = false;
    [HideInInspector]public Monster monsterScript;
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

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        runeManager = FindAnyObjectByType<ManagerRunes>();
        if (monsterSaved !=null)
        {
            monsterScript = monsterSaved.GetComponent<Monster>();
        }
        CheckUse();
    }

    void Update()
    {
        if (isMonsterSelected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RangeArea();
                wasDown = true;
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

    // Método para seleccionar el monstruo al hacer clic en la imagen
    public void SelectMonster()
    {
        if (isUsed || monsterScript.dead) return;
        if (gameManager.monsterSelected == null)
        {
            Debug.Log("Entra en SelectMonster");
            isMonsterSelected = true;
        }
        else
        {
            foreach (var monster in gameManager.monsterDrop)
            {
                if (monster.instantiatedMonster == gameManager.monsterSelected)
                {
                    Debug.Log("No entra nunca");
                    monster.isUsed = false;
                    monster.healthReserve = monster.monsterScript.healthFigth;
                    monster.wasChanged = true;
                    gameManager.lifeBarsFriends[monster.monsterScript.valueI].GetComponent<Image>().sprite = monsterScript.monsterSO.sprite;
                    monsterScript.lifeBar = gameManager.superiorBarFriends[monster.monsterScript.valueI];
                    gameManager.levelFriends[monster.monsterScript.valueI].text = $"Lv.{monsterScript.level}";
                    if (wasChanged)
                    {
                        instantiatedMonster.SetActive(true);
                        instantiatedMonster.transform.position = monster.instantiatedMonster.transform.position;
                        instantiatedMonster.transform.rotation = monster.instantiatedMonster.transform.rotation;
                        instantiatedMonster.GetComponent<Monster>().target = monster.monsterScript.target;
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
                    isMonsterSelected = false; // Resetear para evitar más instancias sin nueva selección
                    isUsed = true;
                    gameManager.friendsList.Add(instantiatedMonster);
                    gameManager.friendsList.Remove(gameManager.monsterSelected);
                    Destroy(gameManager.selectorActive.gameObject);
                    monster.instantiatedMonster.SetActive(false);
                    gameManager.monsterSelected = null;
                    Monster scriptInstantiated = instantiatedMonster.GetComponent<Monster>();
                    monsterScript = scriptInstantiated;
                    monsterScript.valueI = monster.monsterScript.valueI;
                    monsterScript.lifeBar.UpdateFill(monsterScript);
                    Debug.Log(monster.monsterScript);
                    foreach (var enemie in gameManager.enemieList)
                    {
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
        Debug.Log("Entra spanwMonster");
        saveMonsterMarked = null;
        bool isOnButton = IsPointerOverButton();
        if (isOnButton)
        {
            Debug.Log("Is Pointer over Button");
            isMonsterSelected = false;
            areaInstantiatedObj.SetActive(false);
            return;
        }
        // Obtener la posición del clic en el mundo
        areaInstantiatedObj.SetActive(false);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        isMonsterSelected = false; // Resetear para evitar más instancias sin nueva selección
        if (gameManager.countMonsters < 3)
        {
            gameManager.countMonsters++;
            
            if (Physics.Raycast(ray, out hit)) // Para juegos 3D
            {
                if (!wasChanged)
                {
                    instantiatedMonster = Instantiate(monsterSaved, hit.point + Vector3.up * 2, Quaternion.identity);
                    Monster scriptMonster = instantiatedMonster.GetComponent<Monster>();
                    runeManager.AddBuffs(scriptMonster);
                }
                else
                {
                    instantiatedMonster.SetActive(true);
                    instantiatedMonster.transform.position = hit.point + Vector3.up * 2;
                }
                isUsed = true;
                gameManager.friendsList.Add(instantiatedMonster);
                int i = 0;
                foreach (var list in gameManager.lifeBarsFriends)
                {
                    if (list.activeSelf == false)
                    {
                        gameManager.lifeBarsFriends[i].SetActive(true);
                        gameManager.lifeBarsFriends[i].GetComponent<Image>().sprite = monsterScript.monsterSO.sprite;
                        Monster scriptMonster = instantiatedMonster.GetComponent<Monster>();
                        Debug.Log("Entra en el spawn y debería tomar la lifebar");
                        scriptMonster.lifeBar = gameManager.superiorBarFriends[i];
                        gameManager.levelFriends[i].text = $"Lv.{monsterScript.level}";
                        monsterScript.valueI = i;
                        return;
                    }
                    i++;
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
            if (monsterScript.inUse)
            {
                isUsed = true;
                instantiatedMonster = monsterScript.gameObject;
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
        else if(saveMonsterMarked != monsterSelected)
        {
            markInstance.transform.position = monsterSelected.transform.position + Vector3.up * 2;
            markInstance.transform.parent = monsterSelected.transform;
        }
        saveMonsterMarked = monsterSelected;
    }
}
