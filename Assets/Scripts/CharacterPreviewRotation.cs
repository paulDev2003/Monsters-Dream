using UnityEngine;

public class CharacterPreviewRotation : MonoBehaviour
{
    public float rotationSpeed = 100f;

    private bool isDragging = false;
    private Vector3 previousMousePosition;

    void Update()
    {
        // Detectar clic derecho o izquierdo sostenido (puedes ajustar a tu gusto)
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            previousMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 mouseDelta = Input.mousePosition - previousMousePosition;
            float rotationAmount = mouseDelta.x * rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up, -rotationAmount, Space.World);

            previousMousePosition = Input.mousePosition;
        }
    }
}
