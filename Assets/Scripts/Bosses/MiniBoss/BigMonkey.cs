using UnityEngine;

public class BigMonkey : Monster
{

    protected override void Start()
    {
        InitializeStateMachines();
        SetupCore();         // Sí lo quiero
        SetupLists();        // Sí lo quiero
        SetupUI();        // ❌ No quiero mostrar el círculo de ataque
        SetupTarget();       // Sí lo quiero
        SetupExtras();    // ❌ No necesito destruir areaAttack
        //Invoke("IncreaseDistanceAttackFriends", 2f);
    }

    protected override void Update()
    {
        monsterStateMachine.currentState.FrameUpdate();
        /*
        if (HealthFigth <= 0)
        {
            HealthFigth = 0;
            gameManager.specialEvent = false;
            gameManager.RemoveFromList(ownList, this);
            dead = true;

        }
        */
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

