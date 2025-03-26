using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MonsterDrop : MonoBehaviour
{
    public GameObject monsterSaved;
    private bool isMonsterSelected = false; // Bandera para saber si ya se eligi� un monstruo
    public bool isUsed = false;
    [HideInInspector]public Monster monsterScript;
    private GameManager gameManager;
    public GameObject instantiatedMonster;
    private float healthReserve;
    public bool wasChanged = false;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        if (monsterSaved !=null)
        {
            monsterScript = monsterSaved.GetComponent<Monster>();
            Debug.Log("Entr�");
        }
        CheckUse();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detecta clic izquierdo
        {
            // Si ya hay un monstruo guardado, intentar instanciarlo
            if (isMonsterSelected)
            {
                SpawnMonster();
            }
        }
    }

    // M�todo para seleccionar el monstruo al hacer clic en la imagen
    public void SelectMonster()
    {
        if (isUsed) return;
        if (gameManager.monsterSelected == null)
        {
            isMonsterSelected = true;
        }
        else
        {
            foreach (var monster in gameManager.monsterDrop)
            {
                if (monster.instantiatedMonster == gameManager.monsterSelected)
                {
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
                    }
                    else
                    {
                        instantiatedMonster = Instantiate(monsterSaved, monster.instantiatedMonster.transform.position + Vector3.up * 2, monster.transform.rotation);
                    }              
                    isMonsterSelected = false; // Resetear para evitar m�s instancias sin nueva selecci�n
                    isUsed = true;
                    gameManager.friendsList.Add(instantiatedMonster);
                    gameManager.friendsList.Remove(gameManager.monsterSelected);
                    monster.instantiatedMonster.SetActive(false);
                    gameManager.monsterSelected = null;
                    Monster scriptInstantiated = instantiatedMonster.GetComponent<Monster>();
                    foreach (var enemie in gameManager.enemieList)
                    {
                        Monster scriptEnemie = enemie.GetComponent<Monster>();
                        if (scriptEnemie.target == monster.monsterScript)
                        {
                            scriptEnemie.target = scriptInstantiated;
                        }
                    }
                }
            }
        }
    }

    void SpawnMonster()
    {
        // Evitar que el clic sobre la UI instancie el monstruo
        if (EventSystem.current.IsPointerOverGameObject())
        {
            isMonsterSelected = false;
            return;
        }
        // Obtener la posici�n del clic en el mundo
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (gameManager.countMonsters < 3)
        {
            gameManager.countMonsters++;
            
            if (Physics.Raycast(ray, out hit)) // Para juegos 3D
            {
                instantiatedMonster = Instantiate(monsterSaved, hit.point + Vector3.up * 2, Quaternion.identity);
                isMonsterSelected = false; // Resetear para evitar m�s instancias sin nueva selecci�n
                isUsed = true;
                gameManager.friendsList.Add(instantiatedMonster);
                int i = 0;
                foreach (var list in gameManager.lifeBarsFriends)
                {
                    if (list.activeSelf == false)
                    {
                        gameManager.lifeBarsFriends[i].SetActive(true);
                        gameManager.lifeBarsFriends[i].GetComponent<Image>().sprite = monsterScript.monsterSO.sprite;
                        monsterScript.lifeBar = gameManager.superiorBarFriends[i];
                        gameManager.levelFriends[i].text = $"Lv.{monsterScript.level}";
                        monsterScript.valueI = i;
                        return;
                    }
                    i++;
                }
            }
            
        }
        
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
}
