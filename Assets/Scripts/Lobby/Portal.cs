using UnityEngine;
using UnityEngine.Events;

public class Portal : MonoBehaviour
{
    public UnityEvent OpenPortal;

    private void OnMouseDown()
    {
        OpenPortal.Invoke();
    }
}
