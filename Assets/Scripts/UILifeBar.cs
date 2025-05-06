using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UILifeBar : MonoBehaviour
{
    public Image image;
    public List<Image> imgStateEffect = new List<Image>();
    public List<TextMeshProUGUI> txtStateEffect = new List<TextMeshProUGUI>();
    void Start()
    {
       // image = GetComponent<Image>();
    }

    public void UpdateFill(Monster monster)
    {
        image.fillAmount = monster.healthFigth/monster.health;
    }
}
