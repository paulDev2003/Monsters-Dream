using UnityEngine;

public class GiveShieldArea : MonoBehaviour
{
    public bool enemie = false;
    public int shieldUp;
    public StatusEffect effect;
    public SphereCollider sphere;
    public Monster owner;

    public void ActivateArea()
    {
        ApplyShield();
        owner.monsterStateMachine.ChangeState(owner.monsterBasicAttackState);
    }

    private void ApplyShield()
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
                if (enemie && monsterScript.enemie || !enemie && !monsterScript.enemie)
                {
                    monsterScript.shieldActivated = true;
                    monsterScript.shield += shieldUp;
                    monsterScript.UpdateBar();
                    monsterScript.shieldBar.statusEffect = effect;
                }
            }
        }

        Destroy(gameObject, 0.1f);
    }

}
