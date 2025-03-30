using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    private GameManager currentTarget;
    private void OnEnable()
    {
        currentTarget = FindObjectOfType<GameManager>(); // Busca el único GameManager en la escena
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Update Interfaz"))
        {
            currentTarget.UpdateInterface();
        }
        if (GUILayout.Button("Desactive UI"))
        {
            currentTarget.DesactiveUI();
        }
    }

}
