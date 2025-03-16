using UnityEngine;

[CreateAssetMenu(fileName = "ItemMolecule", menuName = "Scriptable Objects/Item/Molecule")]
public class ItemMolecule : ItemSO
{
    private void OnEnable()
    {
        type = typeItem.Molecule;
    }
}
