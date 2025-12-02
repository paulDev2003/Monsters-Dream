using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SummonSlot : MonoBehaviour
{
    public Image img;
    public string monsterName;
    public int level;
    public TeamSelector teamSelector;

    private RectTransform rectTransform;
    private Vector3 originalPosition;
    [SerializeField] private Canvas canvas;
    private bool followingMouse = false;
    private Vector3 lastMousePos;
    public bool isUsed = false;
    private bool isOnSprite = false;
    public void FillOutStats()
    {
        teamSelector.FillOutStats(monsterName, level);
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (isOnSprite && !isUsed)
            {
                teamSelector.AddTeamMember(this);
            }
        }
        if (followingMouse)
        {

            rectTransform.position = Input.mousePosition;

            
        }
        
    }

    public void PickUpSummon()
    {
        if (!teamSelector.dragging && !isUsed)
        {
            followingMouse = true;
            teamSelector.dragging = true;
            teamSelector.summonDragging = this;
        }        
    }


    public void DropSummon()
    {
        followingMouse = false;
        rectTransform.position = originalPosition;
        if (teamSelector.dragging && teamSelector.summonDragging == this && !teamSelector.isInPanel)
        {
            teamSelector.dragging = false;
            teamSelector.summonDragging = null;
        }
    }

    public void EnterSprite()
    {
        isOnSprite = true;
    }
    public void ExitSprite()
    {
        isOnSprite = false;
    }
}
