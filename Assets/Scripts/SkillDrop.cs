using UnityEngine;
using UnityEngine.UI;

public class SkillDrop : MonoBehaviour
{
    public Image cooldownImage;
    public Image targetImage;
    public Monster monsterOwner;
    public bool showingArea;
    public GameObject areaInstantiated;
    public float maxDistance = 5f;
    public float aroundDistance = 2f;
    public LayerMask floorLayer;
    public void ShootSkill()
    {
        if (monsterOwner.monsterSO.skill.instantAttack)
        {
            monsterOwner.ShootSpecialAttack();
        }
        else
        {
            monsterOwner.monsterSO.skill.ShootSkill(monsterOwner);
            showingArea = true;
        }
    }

    private void Update()
    {
        if (showingArea)
        {
            switch (monsterOwner.monsterSO.skill.typeArea)
            {
                case SkillSO.Area.free:
                    break;
                case SkillSO.Area.limited:
                    LimitedArea();
                    break;
                case SkillSO.Area.aroundCharacter:
                    AroundCharacter();
                    break;
                default:
                    break;
            }
            if (Input.GetMouseButtonDown(0))
            {
                monsterOwner.ShootSpecialAttack();
                showingArea = false;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                showingArea = false;
                Destroy(areaInstantiated);
            }
        }       
    }

    private void LimitedArea()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorLayer))
        {
            Vector3 desiredPosition = hit.point;

            Vector3 offset = desiredPosition - monsterOwner.transform.position;
            if (offset.magnitude > maxDistance)
            {
                offset = offset.normalized * maxDistance;
            }

            areaInstantiated.transform.position = monsterOwner.transform.position + offset;
            areaInstantiated.transform.position += Vector3.up * 0.5f; ;
        }
    }

    private void AroundCharacter()
    {
        if (areaInstantiated == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorLayer))
        {
            Vector3 direction = hit.point - monsterOwner.transform.position;

            if (direction.magnitude > 0.1f)
            {
                direction = direction.normalized;
                Vector3 finalPosition = monsterOwner.transform.position + direction * aroundDistance;
                finalPosition.y += 0.5f; // Ajuste visual opcional
                areaInstantiated.transform.position = finalPosition;

                // Opcional: rotar el área para que mire hacia afuera
                areaInstantiated.transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }

}
