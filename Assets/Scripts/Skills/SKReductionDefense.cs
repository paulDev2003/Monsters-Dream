using UnityEngine;

[CreateAssetMenu(fileName = "Reduction Defense", menuName = "Scriptable Objects/Skills/Reduction Defense")]
public class SKReductionDefense : SkillSO
{
    public Sprite reductionDefenseSprite;
    public float decreaseAmount;
    public override void ShootSkill(Monster owner)
    {
        foreach (var sprite in owner.target.spriteStates)
        {
            if (sprite == reductionDefenseSprite)
            {
                foreach (var enemie in owner.oppositeList)
                {
                    Monster scriptEnemie = enemie.GetComponent<Monster>();
                    bool foundEqual = false;
                    foreach (var spriteInEnemie in scriptEnemie.spriteStates)
                    {
                        if (spriteInEnemie == reductionDefenseSprite)
                        {
                            foundEqual = true;
                        }
                    }
                    if (!foundEqual)
                    {
                        scriptEnemie.ApplyStatus(reductionDefenseSprite);
                        scriptEnemie.defense -= decreaseAmount;
                        owner.specialAttack = false;
                        return;
                    }                                       
                }
                owner.target.ApplyStatus(reductionDefenseSprite);
                owner.target.defense -= decreaseAmount;
                owner.specialAttack = false;
                return;
            }
        }
        owner.target.ApplyStatus(reductionDefenseSprite);
        owner.target.defense -= decreaseAmount;
        owner.specialAttack = false;
    }
}
