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
        owner.monsterStateMachine.ChangeState(owner.monsterBasicAttackState);
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
                    MonsterRunningAwayState fearState = new MonsterRunningAwayState(monsterScript, 
                        monsterScript.monsterStateMachine, fearDistance);
                    monstersFeared.Add(monsterScript);
                    monsterScript.monsterStateMachine.ChangeState(fearState);
                }
            }
        }
        model.SetActive(false);
        StartCoroutine(BackToNormality());
        //Destroy(gameObject, 0.1f);
    }

    

    IEnumerator BackToNormality()
    {
        yield return new WaitForSeconds(timeEffect);
        foreach (var monster in monstersFeared)
        {
            if (monster.monsterStateMachine.currentState != monster.monsterDeadState)
            {
                monster.monsterStateMachine.ChangeState(monster.monsterBasicAttackState);
            }            
        }
        Destroy(gameObject);
    }
}
