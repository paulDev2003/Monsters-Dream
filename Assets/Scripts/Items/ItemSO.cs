using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    [Tooltip("Un valor del 1 al 100")]
    public float dropChance;
    public enum typeItem
    {
        Capturable,
        Molecule
    }
    [HideInInspector]public typeItem type;

    public bool RandomDrop()
    {
        float result = Random.Range(0, 100);
        if (result <= dropChance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
//Los items saldr�n cuando eliminemos a los enemigos. De ah� pueden salir varios items repetidos,
//que tendremos que juntarlos. Y los cada item se ir� a una parte del inventario seg�n su tipo