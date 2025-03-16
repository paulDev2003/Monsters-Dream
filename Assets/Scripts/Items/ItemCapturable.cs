using UnityEngine;

[CreateAssetMenu(fileName = "ItemCapturable", menuName = "Scriptable Objects/Item/Capturable")]
public class ItemCapturable : ItemSO
{
    private void OnEnable()
    {
        type = typeItem.Capturable;
    }
}
