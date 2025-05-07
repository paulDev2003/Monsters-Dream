using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UILifeBar : MonoBehaviour
{
    public Image image;
    public List<Image> imgStateEffect = new List<Image>();
    public List<TextMeshProUGUI> txtStateEffect = new List<TextMeshProUGUI>();
    public void UpdateFill(Monster monster)
    {
        image.fillAmount = monster.healthFigth/monster.health;
    }

    public void ShowStates(List<int> acumulationState, List<Sprite> spriteState)
    {
        for (int i = 0; i < acumulationState.Count; i++)
        {
            imgStateEffect[i].enabled = true;
            imgStateEffect[i].sprite = spriteState[i];
            if (acumulationState[i] > 1)
            {
                txtStateEffect[i].enabled = true;
                txtStateEffect[i].text = acumulationState[i].ToString();
            }
        }
    }
}
