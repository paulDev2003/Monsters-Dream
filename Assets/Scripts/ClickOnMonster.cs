using UnityEngine;

public class ClickOnMonster : MonoBehaviour
{
    private Monster parentScript;
    private void Start()
    {
        parentScript = GetComponentInParent<Monster>();
    }
    private void OnMouseDown()
    {
        parentScript.ChangeSelector();
    }
}
