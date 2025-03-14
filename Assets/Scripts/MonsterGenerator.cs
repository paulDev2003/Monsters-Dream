using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class MonsterGenerator : EditorWindow
{
    private string monsterName;
    private GameObject prefabModelo;
    private MonsterSO stats;

    private GUIStyle guiStyle;
    private string pathAndName = "Assets/Prefabs/Monsters/New Monster";
    private string newPathAndName;

    [MenuItem("LucasTools/Monster Generator")]
    static void Init()
    {
        MonsterGenerator window = (MonsterGenerator)GetWindow(typeof(MonsterGenerator));
        Texture2D iconTitle = EditorGUIUtility.Load("BuildSettings.Lumin On") as Texture2D;
        GUIContent titleContent = new GUIContent("Monster Generator", iconTitle);
        window.titleContent = titleContent;
        window.minSize = new Vector2(350, 400);
        window.Show();
    }


    private void OnGUI()
    {
        guiStyle = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = 15, fixedHeight = 40 };
        GUILayout.Label("Data", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        monsterName = EditorGUILayout.TextField("Name", monsterName);
        prefabModelo = (GameObject)EditorGUILayout.ObjectField("Modelo3D", prefabModelo, typeof(GameObject), true);
        stats = (MonsterSO)EditorGUILayout.ObjectField("Stats", stats, typeof(MonsterSO), true);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        DrawHorizontalLine();
        EditorGUILayout.BeginHorizontal();
        // Sección de previsualización
        
        if (GUILayout.Button("Create",guiStyle))
        {
            CreateMonster();
        }
        if (GUILayout.Button("Clear", guiStyle))
        {
            ClearMonster();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawHorizontalLine()
    {
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        rect.height = 1;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }

    private void CreateMonster()
    {
        // Cargar el prefab base del monstruo
        GameObject monsterPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/MonsterDesign/Monster.prefab");

        if (monsterPrefab == null)
        {
            Debug.LogError("No se encontró el prefab del monstruo en 'Assets/Prefabs/MonsterDesign/Monster.prefab'");
            return;
        }

        if (prefabModelo == null)
        {
            Debug.LogError("No se ha asignado un modelo 3D.");
            return;
        }

        // Crear una instancia temporal del prefab base (SIN colocarla en la escena)
        GameObject monsterInstance = (GameObject)PrefabUtility.InstantiatePrefab(monsterPrefab);
        monsterInstance.GetComponent<Monster>().monsterSO = stats;

        GameObject modeloInstance = Instantiate(prefabModelo, monsterInstance.transform);
        modeloInstance.name = "Modelo3D"; 
        modeloInstance.transform.localPosition = Vector3.zero;
        modeloInstance.transform.localRotation = Quaternion.identity;


        string finalPath = AssetDatabase.GenerateUniqueAssetPath($"Assets/Prefabs/Monsters/{monsterName}.prefab");

        PrefabUtility.SaveAsPrefabAsset(monsterInstance, finalPath);
        AssetDatabase.Refresh();

        // Destruir la instancia temporal para evitar residuos
        GameObject.DestroyImmediate(monsterInstance);

        Debug.Log($"Prefab creado en: {finalPath}");
    }
    private void ClearMonster()
    {
        monsterName = "";
        prefabModelo = null;
        stats = null;
    }


    
}
