using UnityEngine;
using System.Collections.Generic;

public abstract class RuneSO : ScriptableObject
{
    public abstract void UsePower(List<GameObject> friendList);
    public abstract void UsePower(Monster monster);
   
}
