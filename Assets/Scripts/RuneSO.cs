using UnityEngine;
using System.Collections.Generic;

public abstract class RuneSO : ScriptableObject
{
    public string txtInfo;

    public abstract void UsePower(List<GameObject> friendList, int level);
    public abstract void UsePower(Monster monster, int level);

    
   
}
