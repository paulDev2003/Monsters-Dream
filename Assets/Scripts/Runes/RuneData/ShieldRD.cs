using UnityEngine;

[CreateAssetMenu(fileName = "ShieldRD", menuName = "RuneDataSO/ShieldRD")]
public class ShieldRD : RuneDataSO
{
    public GreenRune greenRune;

    public override string LoadData(int level)
    {
        int totalShield = (greenRune.shieldIncreased + greenRune.additionalShieldPerLevel * (level - 1));
        txtInfo = $"Shield is activated and increased by {totalShield}";
        finalCost = cost + level * 3;
        return txtInfo;
    }
}
