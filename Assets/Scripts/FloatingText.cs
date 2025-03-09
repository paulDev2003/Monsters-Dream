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
        //textMesh.color = new Color(startColor.r, startColor.g, startColor.b, textMesh.color.a - fadeSpeed * Time.deltaTime); // Hace que se desvanezca


    }
}
