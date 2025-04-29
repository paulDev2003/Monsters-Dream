using UnityEngine;

[CreateAssetMenu(fileName = "MagicRD", menuName = "RuneDataSO/MagicRD")]
public class MagicRD : RuneDataSO
{
    public BlueRune blueRune;


    public override string LoadData(int level)
    {
        int totalDamage = (blueRune.magicDamageIncreased + blueRune.additionalDamagePerLevel * (level - 1));
        Debug.Log(totalDamage);
        txtInfo = $"Magic attack increased by {totalDamage}";
        finalCost = cost + level * 3;
        return txtInfo;
    }
}
