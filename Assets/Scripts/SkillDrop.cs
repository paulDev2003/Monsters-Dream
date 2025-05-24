using UnityEngine;
using UnityEngine.UI;

public class SkillDrop : MonoBehaviour
{
    public Image cooldownImage;
    public Image targetImage;
    public Monster monsterOwner;
    public void ShootSkill()
    {
        monsterOwner.ShootSpecialAttack();
    }
}
