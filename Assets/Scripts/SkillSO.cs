using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO", menuName = "Scriptable Objects/SkillSO")]
public abstract class SkillSO : ScriptableObject
{
    public string skillName;
    public Sprite sprite;
    public bool instantAttack = true;

    public enum Area
    {
        free,
        limited,
        aroundCharacter
    }

    public Area typeArea;

    public abstract void ShootSkill(Monster owner);
    
}
