using UnityEngine;
using UnityEngine.Events;

public class FigthOrb : MonoBehaviour
{
    private bool insideTrigger = false;
    private bool showing = false;
    public UnityEvent ShowPanel;
    public UnityEvent ClosePanel;

    private void Update()
    {
        if (insideTrigger && Input.GetKeyDown(KeyCode.E) && !showing)
        {
            ShowPanel.Invoke();
            showing = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && showing)
        {
            ClosePanel.Invoke();
            showing = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            insideTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            insideTrigger = false;
        }
    }
}
