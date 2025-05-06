using UnityEngine;

public class ExplosiveBanana : MonoBehaviour
{
    public float speed = 4f;
    public float explosionRadius = 3f;
    public int damage = 10;
    public GameObject explosionEffect;
    private Vector3 direction;

    private void Start()
    {
        Destroy(gameObject, 8f);
    }
    public void Initialize(Vector3 shootDirection)
    {
        direction = shootDirection;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Toca Algo");
        // Evita explotar al tocar quien no debe, ajusta según tu lógica
        if (other.CompareTag("Monster"))
        {
            Monster monsterScript = other.GetComponentInParent<Monster>();
            if (!monsterScript.enemie)
            {
                Explode();
            }
        }
    }

    private void Explode()
    {
        // Efecto visual de explosión
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Buscar aliados cercanos
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Monster"))
            {
                Monster monsterScript = hit.GetComponentInParent<Monster>();
                if (!monsterScript.enemie)
                {
                    monsterScript.TakeDamage(damage);
                    monsterScript.gameManager.CheckIfAnyAlive(monsterScript.ownList);
                }
            }
        }

        Destroy(gameObject);
    }
}
