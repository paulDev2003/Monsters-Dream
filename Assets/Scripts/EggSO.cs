using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EggSO", menuName = "Scriptable Objects/Egg")]
public class EggSO : ScriptableObject
{
    public List<ItemSO> typeItems = new List<ItemSO>();
    public List<int> amountItems = new List<int>();
    public int totalPoints;
}
