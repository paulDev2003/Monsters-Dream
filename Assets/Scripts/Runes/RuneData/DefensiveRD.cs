using UnityEngine;

[CreateAssetMenu(fileName = "DefensiveRD", menuName = "RuneDataSO/DefensiveRD")]
public class DefensiveRD : RuneDataSO
{
    public GreyRune greyRune;
    public override string LoadData(int level)
    {
        int totalDefense = (greyRune.physicIncreaseDefense + greyRune.additionalPhysicPerLevel * (level - 1));
        txtInfo = $"Physic defense increased by {totalDefense}";
        finalCost = cost + level * 2;
        return txtInfo;
    }


}
