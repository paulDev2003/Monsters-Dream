using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float fadeSpeed = 1f;
    private TextMeshProUGUI textMesh;
    private Color startColor;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        startColor = textMesh.color;
    }

    private void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime; // Hace que el texto suba
                                                                        // Hacer que el texto mire hacia la cámara
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);


    }
}
