using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO", menuName = "Scriptable Objects/SkillSO")]
public abstract class SkillSO : ScriptableObject
{
    public string skillName;

    public abstract void ShootSkill(Monster owner);
}
