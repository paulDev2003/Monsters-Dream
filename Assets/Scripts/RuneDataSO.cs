using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "RuneDataSO", menuName = "Scriptable Objects/RuneDataSO")]
public abstract class RuneDataSO : ScriptableObject
{
    public string runeName;
    public int cost;
    public int finalCost;
    public float fontSize;
    public GameObject runePrefab;

    public abstract string LoadData(int level);
}
