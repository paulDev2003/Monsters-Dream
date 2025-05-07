using UnityEngine;

[CreateAssetMenu(fileName = "SKCallPackWolves", menuName = "Scriptable Objects/Skills/CallPackWolves")]
public class SKCallPackWolves : SkillSO
{
    public GameObject prefabMiniWolf;
    public override void ShootSkill(Monster owner)
    {
        int count = 3;
        for (int i = 0; i < count; i++)
        {
            GameObject miniWolf = Instantiate(prefabMiniWolf, owner.gameManager.enemySpawnPoints[i].position, Quaternion.identity);
            owner.gameManager.enemieList.Add(miniWolf);
        }
    }
}
