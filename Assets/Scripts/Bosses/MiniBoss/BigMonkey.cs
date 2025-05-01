using UnityEngine;

public class BigMonkey : Monster
{

    protected override void Start()
    {
        SetupCore();         // Sí lo quiero
        SetupLists();        // Sí lo quiero
        // SetupUI();        // ❌ No quiero mostrar el círculo de ataque
        SetupTarget();       // Sí lo quiero
        // SetupExtras();    // ❌ No necesito destruir areaAttack
        //Invoke("IncreaseDistanceAttackFriends", 2f);
    }

    protected override void Update()
    {
        if (healthFigth <= 0)
        {
            healthFigth = 0;
            gameManager.specialEvent = false;
            gameManager.RemoveFromList(ownList, this);
            dead = true;

        }
    }
    protected override void Attack()
    {
        // El minijefe no ataca normalmente
    }

    public void IncreaseDistanceAttackFriends()
    {
        foreach (var monster in gameManager.friendsList)
        {
            Monster monsterScript = monster.GetComponent<Monster>();
            monsterScript.distanceAttack *= 2;
        }
    }
}

