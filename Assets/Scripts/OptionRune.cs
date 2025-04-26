using UnityEngine;
using TMPro;

public class OptionRune : MonoBehaviour
{
    public TextMeshProUGUI txtLevel;
    public TextMeshProUGUI txtDescription;
    public TextMeshProUGUI txtCost;
    public RectTransform spotPrefab;
    public Rune rune;

    public void SelectRune()
    {
        rune.SelectRune();
    }
}
