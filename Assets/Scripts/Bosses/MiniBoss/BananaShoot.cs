using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BananaShoot : MonoBehaviour
{
    public float timeCooldown;
    public float currentTimeCooldown;
    public float timeToShoot;
    public Transform spawnShootPoint;
    public Transform spawnArea;
    public GameObject trayectShoot;
    public GameObject explosiveBanana;
    public GameManager gameManager;
    public float rotationSpeed;
    private GameObject target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTimeCooldown < timeCooldown)
        {
            currentTimeCooldown += Time.deltaTime;
        }
        else if (!gameManager.finish)
        {
            ShootBanana();
            currentTimeCooldown = 0;
        }
        if (target!=null)
        {
            RotateTowardsTarget();
        }
    }

    private void ShootBanana()
    {
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        GameObject monsterChoose = gameManager.friendsList[Random.Range(0, gameManager.friendsList.Count)];
        if (monsterChoose == null)
        {
            gameManager.CheckIfAnyAlive(gameManager.friendsList);
            StopCoroutine(ShootRoutine());
        }
        target = monsterChoose;
        GameObject trajectory = Instantiate(trayectShoot, spawnArea.position, Quaternion.identity);

        Vector3 direction = (monsterChoose.transform.position - spawnArea.position).normalized;

        trajectory.transform.rotation = Quaternion.LookRotation(direction);
        trajectory.transform.Rotate(0f, 90f, 0f);
        yield return new WaitForSeconds(timeToShoot);
        direction = (monsterChoose.transform.position - spawnArea.position).normalized;
        GameObject proyectil = Instantiate(explosiveBanana, spawnShootPoint.position, Quaternion.identity);
        proyectil.transform.rotation = Quaternion.LookRotation(direction);
        proyectil.GetComponent<ExplosiveBanana>().Initialize(direction);
        yield return new WaitForSeconds(0.5f);
        Destroy(trajectory);
    }

    private void RotateTowardsTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction = new Vector3(direction.x, 0, direction.z);
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
