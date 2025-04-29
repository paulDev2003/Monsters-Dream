using UnityEngine;

public class UILineRenderer : MonoBehaviour
{
    public RectTransform pointAUI;        // Elemento UI din�mico (punto A)
    public RectTransform pointBUI;        // Elemento UI est�tico (punto B)
    public LineRenderer lineRenderer;
    public Canvas canvas;                 // Tu Canvas (opcional si ya lo tienes referenciado)

    void Update()
    {
        Vector3 uiPosA = pointAUI.position;
        Vector3 uiPosB = pointBUI.position;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, uiPosA);
        lineRenderer.SetPosition(1, uiPosB);
    }
}
