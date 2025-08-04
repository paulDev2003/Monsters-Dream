using UnityEngine;

public class MonkeyHelperCheck : MonoBehaviour
{
    public Monster monsterScript;
    public GameManager gameManager;
    public GameObject bigMonkey;
    public GameObject lifeBar;
    public bool activated = false;
    public bool scriptObtained = false;
    public bool checkAlive = false;

    private void Update()
    {
        if (gameManager.enemieList.Count == 0 && !activated)
        {
            ActivateBigMonkey();
            activated = true;
        }
        if (monsterScript.HealthFigth <= 0 && activated && !checkAlive)
        {
            lifeBar.SetActive(false);
            gameManager.specialEvent = false;
            gameManager.CheckIfAnyAlive(monsterScript.ownList);
            checkAlive = true;
        }
    }

    public void ActivateBigMonkey()
    {
        gameManager.enemieList.Add(bigMonkey);
        bigMonkey.GetComponentInChildren<BigMonkey>().enabled = true;
        lifeBar.SetActive(true);
    }
}
