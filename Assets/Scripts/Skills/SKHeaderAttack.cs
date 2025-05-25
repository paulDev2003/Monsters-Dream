using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Header Attack", menuName = "Scriptable Objects/Skills/Header Attack")]
public class SKHeaderAttack : SkillSO
{
    public float distanceBehind = 1.5f;
    public float moveSpeed = 5f;
    public int damage;

    public override void ShootSkill(Monster owner)
    {
        owner.RunSkillCoroutine(MoveBehindTarget(owner));
    }

    private IEnumerator MoveBehindTarget(Monster owner)
    {
        if (owner.target == null) yield break;
        owner.specialAttack = true;
        owner.agent.enabled = false;
        Vector3 behindPosition = owner.target.transform.position- owner.target.transform.forward * distanceBehind;

        while (Vector3.Distance(owner.transform.position, behindPosition) > 0.05f)
        {
            owner.transform.position = Vector3.MoveTowards(
                owner.transform.position,
                behindPosition,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
        owner.target.TakeDamage(damage);
        owner.target.UpdateBar();
        owner.agent.enabled = true;
        owner.specialAttack = false;
        Debug.Log("Habilidad completada: llegó detrás del target");
        // Aquí puedes aplicar efectos, daño, animaciones, etc.
    }
}
