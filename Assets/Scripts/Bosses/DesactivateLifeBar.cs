using UnityEngine;

public class DesactivateLifeBar : MonoBehaviour
{
    public GameObject lifeBar;
    public GameManager gameManager;
    private void Update()
    {
        if (gameManager.finish)
        {
            lifeBar.SetActive(false);
        }
    }
}
