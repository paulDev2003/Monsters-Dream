using UnityEngine;
using UnityEngine.UI;

public class UILifeBar : MonoBehaviour
{
    public Image image;
    void Start()
    {
       // image = GetComponent<Image>();
    }

    public void UpdateFill(Monster monster)
    {
        image.fillAmount = monster.healthFigth/monster.health;
    }
}
