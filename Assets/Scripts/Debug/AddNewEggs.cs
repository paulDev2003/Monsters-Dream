using UnityEngine;
using System.Collections.Generic;

public class AddNewEggs : MonoBehaviour
{
    public List<DiscoverMonster> eggsToAdd = new List<DiscoverMonster>();
    public MonstersHouse monstersHouse;
    public ManagerEggSpots managerEggSpots;

    public void AddToBestiary()
    {
        foreach (var egg in eggsToAdd)
        {
            bool isInBestiary = false;
            foreach (var monster in monstersHouse.bestiary)
            {
                if (monster.monsterName == egg.monsterName)
                {
                    monster.wasFriend = true;
                    isInBestiary = true;
                    break;
                }
            }
            if (!isInBestiary)
            {
                monstersHouse.bestiary.Add(egg);
            }
        }
        managerEggSpots.CheckEggsAvailable();
    }
}
