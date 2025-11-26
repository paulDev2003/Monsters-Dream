using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EggSpot : MonoBehaviour
{
    public ManagerEggSpots managerEggSpots;
    public bool available = true;
    public Transform spawnEgg;
    public GameObject egg;
    public Camera eggsCamera;
    public GameObject progressBar;
    public Image imgSuperiorBar;
    public EggPanel eggPanel;
    public MenuTutorial lobbyTutorial;
    public UnityEvent TutorialEvent;
    public int id;
    public string monsterName;
    private void OnMouseDown()
    {
        if (managerEggSpots.chooseEgg)
        {
            if (available)
            {
                egg = managerEggSpots.eggInstantiated;
                egg.transform.position = spawnEgg.transform.position;
                Egg scriptEgg = egg.GetComponent<Egg>();
                scriptEgg.eggData.id = id;
                scriptEgg.eggSpot = this;
                monsterName = scriptEgg.eggData.monsterName;
                available = false;
                eggsCamera.enabled = false;
                progressBar.SetActive(true);
                imgSuperiorBar.fillAmount = 0.01f;
                eggPanel.eggs.Add(scriptEgg);
                managerEggSpots.chooseEgg = false;
                eggPanel.SaveEggs();
                if (!lobbyTutorial.hasMadeTutorial)
                {
                    TutorialEvent.Invoke();
                }
                managerEggSpots.ActiveUI.Invoke();
                managerEggSpots.eggInstantiated = null;
                if (managerEggSpots.monstersToEgg.Count != 0)
                {
                    managerEggSpots.ChangeEgg(0);
                }            
            }
        }
    }
}
