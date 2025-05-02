using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UpgradeDataBase", menuName = "Scriptable Objects/UpgradeDataBase")]
public class UpgradeDataBase : ScriptableObject
{
    public List<UpgradeBase> allUpgrades;

    private Dictionary<string, UpgradeBase> upgradeDict;

    public void Initialize()
    {
        upgradeDict = new Dictionary<string, UpgradeBase>();
        foreach (var upgrade in allUpgrades)
        {
            if (!upgradeDict.ContainsKey(upgrade.upgradeName))
                upgradeDict.Add(upgrade.upgradeName, upgrade);
        }
    }

    public UpgradeBase GetUpgradeBaseByName(string upgradeName)
    {
        if (upgradeDict == null)
            Initialize();
        return upgradeDict.TryGetValue(upgradeName, out var upgradeBase) ? upgradeBase : null;
    }
}
