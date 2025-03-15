using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Monster))]
public class MonsterEditor : Editor
{
    private Monster currentTarget;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        currentTarget = (Monster)target;
        if (GUILayout.Button("UpdateStats"))
        {
            currentTarget.UpdateStats();
        }
    }
}
