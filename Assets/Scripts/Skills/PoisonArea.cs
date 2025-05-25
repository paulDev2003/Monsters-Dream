using UnityEngine;

public class PoisonArea : MonoBehaviour
{
    public bool enemie = false;
    public StatusEffect poisonEffect;
    public int damage;
    public BoxCollider box;


    public void ActivateArea()
    {
        ApplyPoisonDamage();
    }

    private void ApplyPoisonDamage()
    {
        Vector3 center = transform.TransformPoint(box.center);
        Vector3 halfExtents = Vector3.Scale(box.size, transform.lossyScale) / 2f;
        Quaternion rotation = transform.rotation;

        Collider[] hits = Physics.OverlapBox(center, halfExtents, rotation);
        foreach (var collision in hits)
        {
            Monster monsterScript = collision.GetComponentInParent<Monster>();
            if (monsterScript != null)
            {
                Debug.Log("Encuentra el monstruo: " + monsterScript.name);
                if (enemie && !monsterScript.enemie || !enemie && monsterScript.enemie)
                {
                    monsterScript.TakeDamage(damage);
                    monsterScript.AddStatusEffect(poisonEffect);
                }
            }
        }
        Destroy(gameObject, 0.1f);
    }

    private void OnDrawGizmos()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        if (box == null) return;

        // Calcular el centro y tamaño real en mundo
        Vector3 center = transform.TransformPoint(box.center);
        Vector3 halfExtents = Vector3.Scale(box.size, transform.lossyScale) / 2f;

        // Aplicar la rotación del objeto
        Quaternion rotation = transform.rotation;

        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2f); // Dibuja el cubo completo
    }
}


