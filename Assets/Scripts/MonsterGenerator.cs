using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class MonsterGenerator : EditorWindow
{
    [MenuItem("Tools/Monster Generator")]
    static void Init()
    {
        MonsterGenerator window = (MonsterGenerator)GetWindow(typeof(MonsterGenerator));
        Texture2D iconTitle = EditorGUIUtility.Load("BuildSettings.Lumin On") as Texture2D;
        GUIContent titleContent = new GUIContent("Monster Generator", iconTitle);
        window.titleContent = titleContent;
        window.minSize = new Vector2(350, 250);
        window.Show();
    }
}
