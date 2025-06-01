using UnityEngine;
using UnityEngine.UI;

public class EggSpot : MonoBehaviour
{
    public Bestiary bestiary;
    private bool available = true;
    public Transform spawnEgg;
    private GameObject egg;
    public Camera eggsCamera;
    public GameObject progressBar;
    public Image imgSuperiorBar;

    void Update()
    {
        if (true)
        {

        }
    }

    private void OnMouseDown()
    {
        if (bestiary.chooseEgg)
        {
            if (available)
            {
                egg = Instantiate(bestiary.eggInstantiated, spawnEgg.position, bestiary.eggInstantiated.transform.rotation);
                available = false;
                eggsCamera.enabled = false;
                progressBar.SetActive(true);
                imgSuperiorBar.fillAmount = 0.01f;
            }
        }
    }
}
