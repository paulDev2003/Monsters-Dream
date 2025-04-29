using UnityEngine;
using TMPro;

public abstract class RuneDataSO : ScriptableObject
{
    public string runeName;
    public int cost;
    public int finalCost;
    public float fontSize;
    public GameObject runePrefab;
    public string txtInfo;

    public abstract string LoadData(int level);
}
