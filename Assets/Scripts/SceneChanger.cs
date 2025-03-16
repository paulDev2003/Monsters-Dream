using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private UnityEditor.SceneAsset sceneAsset; // Referencia en el Inspector
    private string sceneName;

    private void Start()
    {
        if (sceneAsset != null)
        {
            sceneName = sceneAsset.name; // Guardamos el nombre de la escena
        }
    }

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("No se ha asignado una escena en el Inspector.");
        }
    }
}