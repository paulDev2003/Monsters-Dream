using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Increase Size", menuName = "Scriptable Objects/Skills/Increase Size")]
public class SKIncreaseSize : SkillSO
{
    public int defenseIncrease;
    public int magicDefenseIncrease;
    public override void ShootSkill(Monster owner)
    {
        owner.defense += defenseIncrease;
        owner.magicalDefense += magicDefenseIncrease;
        AbsorbTargets(owner.oppositeList, owner);
        owner.RunSkillCoroutine(Grow(owner));
        owner.ApplyStatus(sprite);
    }

    private void AbsorbTargets(List<GameObject> oppositeList, Monster owner)
    {
        foreach (GameObject monster in oppositeList)
        {
            Monster monsterScript = monster.GetComponent<Monster>();
            monsterScript.target = monsterScript.ChooseTarget(owner.gameObject);
        }
    }
    private IEnumerator Grow(Monster owner)
    {
        float elapsed = 0f;
        Vector3 finalScale = Vector3.one * 2f;
        float duration = 1f;
        BoxCollider boxCollider = owner.GetComponentInChildren<BoxCollider>();
        Vector3 initialScale = owner.transform.localScale;
        Vector3 initialCenter = boxCollider.center;
        
        Vector3 initialColliderSize = boxCollider.size;
        Vector3 finalColliderSize = initialColliderSize * (finalScale.x / initialScale.x);
        float heightDiff = finalColliderSize.y - initialColliderSize.y;
        Vector3 finalCenter = initialCenter + new Vector3(0f, heightDiff / 2f, 0f);

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            // Escalar visualmente
            owner.transform.localScale = Vector3.Lerp(initialScale, finalScale, t);

            // Ajustar collider
            if (boxCollider != null)
            {
                boxCollider.size = Vector3.Lerp(initialColliderSize, finalColliderSize, t);
                boxCollider.center = Vector3.Lerp(initialCenter, finalCenter, t);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Asegura valores finales exactos
        owner.transform.localScale = finalScale;
        if (boxCollider != null)
        {
            boxCollider.size = finalColliderSize;
            boxCollider.center = finalCenter;
        }
        owner.specialAttack = false;
    }
}
