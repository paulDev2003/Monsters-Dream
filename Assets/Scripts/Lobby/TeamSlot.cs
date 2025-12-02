using UnityEngine;
using UnityEngine.UI;

public class TeamSlot : MonoBehaviour
{
    public Image img;
    public string monsterName;
    public int level;
    public TeamSelector teamSelector;
    private bool isOnSprite = false;
    public int id;
    private bool followingMouse = false;
    private RectTransform rectTransform;
    private Vector3 originalPosition;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (isOnSprite)
            {
                followingMouse = false;
                isOnSprite = false;
                teamSelector.DeleteTeamMember(this);
            }
        }
        if (followingMouse)
        {
            rectTransform.position = Input.mousePosition;
        }
    }
    public void FillOutStats()
    {
        teamSelector.FillOutStats(monsterName, level);
    }

    public void EnterSprite()
    {
        isOnSprite = true;
    }

    public void ExitSprite()
    {
        isOnSprite = false;
    }

    public void PickUpSummon()
    {
        followingMouse = true;
        teamSelector.draggingTeamSlot = true;
        teamSelector.teamSlotDragging = this;
        transform.SetSiblingIndex(0);
    }

    public void DropSummon()
    {
        followingMouse = false;
        rectTransform.position = originalPosition;
       // teamSelector.draggingTeamSlot = false;
    }
    public void ChangeSummon()
    {
        teamSelector.ChangeTeamMember(this);
    }

    public void ChangeSlot()
    {
        teamSelector.ChangeTeamSlot(this);
    }
}
