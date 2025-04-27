using UnityEngine;

[CreateAssetMenu(fileName = "SpeedRD", menuName = "RuneDataSO/SpeedRD")]
public class SpeedRD : RuneDataSO
{
    public BlackRune blackRune;
    public string txtInfo;
    public override string LoadData(int level)
    {
        float totalSA = (blackRune.multiplierSpeedAttack + blackRune.additionalSA_perLevel * (level - 1));
        float totalCoolDown = (blackRune.decreasedCoolDownPercentage + blackRune.additionalCDP_perLevel * (level - 1));
        txtInfo = $"Speed Attack increased by {totalSA} and Cooldown attacks decreased by {totalCoolDown}%";
        finalCost = cost + level * 2;
        return txtInfo;
    }
}
