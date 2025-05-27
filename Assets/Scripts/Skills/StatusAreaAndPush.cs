using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class StatusAreaAndPush : MonoBehaviour
{
    public bool enemie = false;
    public StatusEffect poisonEffect;
    public int damage;
    public BoxCollider box;
    public float pushForce = 5f; // Fuerza de empuje

    public void ActivateArea()
    {
        ApplyStatusDamage();
    }

    private void ApplyStatusDamage()
    {
        Vector3 center = transform.TransformPoint(box.center);
        Vector3 halfExtents = Vector3.Scale(box.size, transform.lossyScale) / 2f;
        Quaternion rotation = transform.rotation;

        Collider[] hits = Physics.OverlapBox(center, halfExtents, rotation);
        foreach (var collision in hits)
        {
            Monster monsterScript = collision.GetComponentInParent<Monster>();
            if (monsterScript == null) continue;

            if ((enemie && !monsterScript.enemie) || (!enemie && monsterScript.enemie))
            {
                monsterScript.TakeDamage(damage);
                monsterScript.AddStatusEffect(poisonEffect);

                Transform targetTransform = monsterScript.transform;
                NavMeshAgent agent = monsterScript.GetComponent<NavMeshAgent>();

                Vector3 direction = (targetTransform.position - center).normalized;
                Vector3 pushTarget = targetTransform.position + direction * 2f;

                if (agent != null)
                {
                    StartCoroutine(PushAndReactivate(agent, targetTransform, pushTarget, 5f));
                }
            }
        }

        
    }

    private IEnumerator PushAndReactivate(NavMeshAgent agent, Transform obj, Vector3 targetPos, float speed)
    {
        agent.enabled = false;

        while (Vector3.Distance(obj.position, targetPos) > 0.05f)
        {
            obj.position = Vector3.MoveTowards(obj.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }

        obj.position = targetPos;

        // Espera un frame para que Unity registre bien la posición antes de reactivar el agente
        yield return null;

        agent.enabled = true;
        agent.ResetPath();
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        if (box == null) return;

        Vector3 center = transform.TransformPoint(box.center);
        Vector3 halfExtents = Vector3.Scale(box.size, transform.lossyScale) / 2f;
        Quaternion rotation = transform.rotation;

        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2f);
    }
}
