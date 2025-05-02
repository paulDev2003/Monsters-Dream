using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeBase", menuName = "Scriptable Objects/UpgradeBase")]
public class UpgradeBase : ScriptableObject
{
    public string upgradeName;
    public Upgrade upgradeSO;
}
