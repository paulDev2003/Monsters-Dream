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

    private PreviewRenderUtility previewUtility;
    private GameObject targetObject;

    private Quaternion modelRotation = Quaternion.identity;
    private Vector2 drag;
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



    private void OnEnable()
    {
        previewUtility = new PreviewRenderUtility();
    }

    private void OnDisable()
    {
        if (previewUtility != null)
            previewUtility.Cleanup();

        if (targetObject != null)
            Object.DestroyImmediate(targetObject);
    }

    private void SetupPreviewScene()
    {
        if (targetObject != null && !EditorUtility.IsPersistent(targetObject))
        {
            Object.DestroyImmediate(targetObject);
        }
        targetObject = Instantiate(prefabModelo);
        targetObject.transform.position = Vector3.zero;
        targetObject.hideFlags = HideFlags.HideAndDontSave;
        previewUtility.AddSingleGO(targetObject);
        previewUtility.camera.transform.position = new Vector3(0f, 0f, -20f);
        previewUtility.camera.nearClipPlane = 2f;
        previewUtility.camera.farClipPlane = 30f;
    }
    private void Update()
    {
        Quaternion rotation = Quaternion.Euler(drag.y, drag.x, 0);
        if (targetObject != null)
        {
            targetObject.transform.rotation = rotation;
        }
        Repaint();
    }
    private void HandleMouseInput()
    {
        if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
        {
            drag.x -= Event.current.delta.x;
            drag.y += Event.current.delta.y;
            Event.current.Use();
        }
    }
    private void OnGUI()
    {
        guiStyle = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = 15, fixedHeight = 40 };
        GUILayout.Label("Data", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        monsterName = EditorGUILayout.TextField("Name", monsterName);
        prefabModelo = (GameObject)EditorGUILayout.ObjectField("Modelo3D", prefabModelo, typeof(GameObject), true);
        stats = (MonsterSO)EditorGUILayout.ObjectField("Stats", stats, typeof(MonsterSO), true);
        HandleMouseInput();
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
        float width = base.position.width; // Ancho completo de la ventana
        float height = base.position.height * 0.4f; // El 20% de la altura total de la ventana
        float y = base.position.height - height; // Posicionar en la parte inferior de la ventana


        // Crear el rectángulo con las nuevas dimensiones
        Rect rect = new Rect(0, y, width, height);
        previewUtility.BeginPreview(rect, previewBackground: GUIStyle.none);
        previewUtility.Render();
        var texture = previewUtility.EndPreview();

        GUI.DrawTexture(rect, texture);
        if (GUILayout.Button("Preview"))
        {
            SetupPreviewScene();
        }
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
        modeloInstance.AddComponent<ClickOnMonster>();
        if (modeloInstance.GetComponent<Rigidbody>() == null)
        {
            modeloInstance.AddComponent<Rigidbody>();
        }
        if (modeloInstance.GetComponent<BoxCollider>() == null)
        {
            modeloInstance.AddComponent<BoxCollider>();
        }


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
