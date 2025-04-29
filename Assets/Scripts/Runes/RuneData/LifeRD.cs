using UnityEngine;

[CreateAssetMenu(fileName = "LifeRD", menuName = "RuneDataSO/LifeRD")]
public class LifeRD : RuneDataSO
{
    public GreenRune greenRune;
    public override string LoadData(int level)
    {
        int totalRegeneration = (greenRune.healthRegeneration + greenRune.additionalRegenerationPerLevel * (level - 1));
        txtInfo = $"Health regeneration increased by {totalRegeneration}";
        finalCost = cost + level * 2;
        return txtInfo;
    }
}
