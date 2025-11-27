using UnityEngine;
using UnityEngine.UI;

public class UnlockedSummonUI : MonoBehaviour
{
    public Image img;
    public Sprite sprite;
    public string monsterName;
    
    public void ActivateCard()
    {
        img.sprite = sprite;
    }
}
