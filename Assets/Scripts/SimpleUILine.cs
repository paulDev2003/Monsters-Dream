using UnityEngine;

public class SimpleUILine : MonoBehaviour
{
    public RectTransform destinyPoint;
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }


    public void ConectLine(RectTransform pointA)
    {
        destinyPoint.gameObject.SetActive(true);
        Vector3 localPosA = pointA.localPosition;
        Vector3 localPosB = destinyPoint.localPosition;

        Vector3 direction = localPosB - localPosA;
        float distance = direction.magnitude;

        rectTransform.localPosition = localPosA + direction / 2f;
        rectTransform.sizeDelta = new Vector2(distance, 4f); // grosor de línea = 4
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
    }
}
