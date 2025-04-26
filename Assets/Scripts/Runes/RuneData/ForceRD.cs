using UnityEngine;

[CreateAssetMenu(fileName = "ForceRD", menuName = "RuneDataSO/ForceRD")]
public class ForceRD : RuneDataSO
{
    public RedRune redRune;
    public string txtInfo = $"Basic Damage increased by ";

    public override string LoadData(int level)
    {
        int totalDamage = (redRune.basicDamageIncreased + redRune.basicDamagePerLevel * (level - 1));
        Debug.Log(totalDamage);
        txtInfo = $"Basic Damage increased by {totalDamage}";
        finalCost =  cost + level * 2;
        return txtInfo;
    }
}
