using UnityEngine;

public class HealthArea : MonoBehaviour
{
    public bool enemie = false;
    public int healthUp;
    public SphereCollider sphere;
    public Monster owner;

    public void ActivateArea()
    {
        ApplyHealth();
        owner.monsterStateMachine.ChangeState(owner.monsterBasicAttackState);
    }

    private void ApplyHealth()
    {
        // Obtener el centro del collider en el mundo
        Vector3 center = transform.TransformPoint(sphere.center);

        // Calcular el radio considerando el escalado del objeto
        float maxScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        float radius = sphere.radius * maxScale;

        Collider[] hits = Physics.OverlapSphere(center, radius);
        foreach (var collision in hits)
        {
            Monster monsterScript = collision.GetComponentInParent<Monster>();
            if (monsterScript != null)
            {
                Debug.Log("Encuentra el monstruo: " + monsterScript.name);
                if (enemie && monsterScript.enemie || !enemie && !monsterScript.enemie)
                {
                    monsterScript.RegenerateHealth(healthUp);
                }
            }
        }

        Destroy(gameObject, 0.1f);
    }

}
