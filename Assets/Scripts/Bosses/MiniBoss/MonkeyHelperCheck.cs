using UnityEngine;

public class MonkeyHelperCheck : MonoBehaviour
{
    public Monster monsterScript;
    public GameManager gameManager;
    public GameObject bigMonkey;
    public GameObject lifeBar;
    public bool activated = false;
    public bool scriptObtained = false;

    private void Update()
    {
        if (gameManager.enemieList.Count == 1)
        {
            if (!scriptObtained)
            {
                monsterScript = gameManager.enemieList[0].GetComponent<Monster>();
                scriptObtained = true;
            }
            if (monsterScript.healthFigth <= 2 && !activated)
            {
                ActivateBigMonkey();
                monsterScript.healthFigth = 2;
                activated = true;
            }
        }
        if (gameManager.finish)
        {
            lifeBar.SetActive(false);
        }
    }

    public void ActivateBigMonkey()
    {
        gameManager.enemieList.Add(bigMonkey);
        bigMonkey.GetComponentInChildren<BigMonkey>().enabled = true;
        lifeBar.SetActive(true);
    }
}
