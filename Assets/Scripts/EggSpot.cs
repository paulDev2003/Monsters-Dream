using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EggSpot : MonoBehaviour
{
    public Bestiary bestiary;
    public bool available = true;
    public Transform spawnEgg;
    private GameObject egg;
    public Camera eggsCamera;
    public GameObject progressBar;
    public Image imgSuperiorBar;
    public EggPanel eggPanel;
    public MenuTutorial lobbyTutorial;
    public UnityEvent TutorialEvent;
    public int id;

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
                Egg scriptEgg = egg.GetComponent<Egg>();
                scriptEgg.eggData.id = id;
                scriptEgg.eggSpot = this;
                available = false;
                eggsCamera.enabled = false;
                progressBar.SetActive(true);
                imgSuperiorBar.fillAmount = 0.01f;
                eggPanel.eggs.Add(scriptEgg);
                bestiary.chooseEgg = false;
                eggPanel.SaveEggs();
                if (!lobbyTutorial.hasMadeTutorial)
                {
                    TutorialEvent.Invoke();
                }
            }
        }
    }
}
