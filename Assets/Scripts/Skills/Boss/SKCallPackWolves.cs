using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SKCallPackWolves", menuName = "Scriptable Objects/Skills/CallPackWolves")]
public class SKCallPackWolves : SkillSO
{
    public GameObject prefabMiniWolf;
    public int countToSpawn = 3;
    private bool canSpawn = true;
    public override void ShootSkill(Monster owner)
    {
        if (owner.gameManager.enemieList.Count != 1)
        {
            canSpawn = false;
        }
        else
        {
            canSpawn = true;
        }

        if (canSpawn)
        {
            for (int i = 0; i < countToSpawn; i++)
            {
                GameObject miniWolf = Instantiate(prefabMiniWolf, owner.gameManager.enemySpawnPoints[i].position, Quaternion.identity);
                owner.gameManager.enemieList.Add(miniWolf);
                Monster scriptWolf = miniWolf.GetComponent<Monster>();
                scriptWolf.enemie = true;
                owner.gameManager.lifeBarsEnemies[i].SetActive(true);
                owner.gameManager.lifeBarsEnemies[i].GetComponent<Image>().sprite = scriptWolf.monsterSO.sprite;
                scriptWolf.lifeBar = owner.gameManager.superiorBarEnemies[i];
                owner.gameManager.levelEnemies[i].text = $"Lv.{scriptWolf.level}";
                scriptWolf.UpdateBar();
            }
        }    
    }
}
