using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class FearArea : MonoBehaviour
{
    public SphereCollider sphere;
    public bool enemie;
    public float fearDistance = 6f; // Cuánto se aleja
    public float randomOffset = 5f; // Cuánta variación aleatoria permites
    public float timeEffect = 4f;
    public Monster owner;
    public List<Monster> monstersFeared = new List<Monster>();
    public GameObject model;

    public void ActivateArea()
    {
        ApplyFear();
    }

    private void ApplyFear()
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
                if (enemie && !monsterScript.enemie || !enemie && monsterScript.enemie)
                {
                    monstersFeared.Add(monsterScript);
                    RunAway(monsterScript);
                }
            }
        }
        model.SetActive(false);
        StartCoroutine(BackToNormality());
        //Destroy(gameObject, 0.1f);
    }

    public void RunAway(Monster affectedMonster)
    {
        affectedMonster.underControl = true;
        // Dirección opuesta al miedo
        Vector3 fleeDirection = (transform.position - affectedMonster.transform.position).normalized;

        // Añadir algo de aleatoriedad al movimiento
        Vector3 random = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        Vector3 finalDirection = (fleeDirection + random * 0.3f).normalized;

        Vector3 targetPos = transform.position + finalDirection * fearDistance;

        // Verificar si el punto está en el NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 5f, NavMesh.AllAreas))
        {
            affectedMonster.agent.SetDestination(hit.position);
        }
    }

    IEnumerator BackToNormality()
    {
        yield return new WaitForSeconds(timeEffect);
        foreach (var monster in monstersFeared)
        {
            monster.underControl = false;
        }
        Destroy(gameObject);
    }
}
