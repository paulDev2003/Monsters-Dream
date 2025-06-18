using UnityEngine;

public class BoxRune : MonoBehaviour
{
    public int horizontalPosition;
    public int verticalPosition;
    private ManagerRunes managerRunes;
    private Rune rune;
    private bool canDrop = false;

    private void Start()
    {
        managerRunes = FindAnyObjectByType<ManagerRunes>();
    }
    public void CheckSlots()
    {
        bool checker = true;
        if (managerRunes.runeSelected == null)
        {
            return;
        }
        rune = managerRunes.runeSelected;
        foreach (var slot in rune.positionsSlots)
        {
            bool validate = ValidatePosition(horizontalPosition, verticalPosition, slot);
            if (!validate)
            {
                Debug.Log("Invalidate Position");
                checker = false;
            }
            else
            {
                if (!managerRunes.slotsChecker[horizontalPosition + slot.x, verticalPosition + slot.y])
                {
                    Debug.Log("Slots en false");
                    checker = false;
                }
            }           
        }
        if (checker)
        {
            PositiveCheck();
        }
        else
        {
            NegativeCheck();
        }
    }

    public void DropRune()
    {
        Debug.Log("Dropea");
        if (canDrop)
        {            
            if (rune.isUsed)
            {
                foreach (var slot in rune.positionsSlots)
                {
                    managerRunes.slotsChecker[rune.savePosition.x + slot.x, rune.savePosition.y + slot.y] = true;
                }
                rune.savePosition = new Vector2Int(horizontalPosition, verticalPosition);
            }
            else
            {
                rune.savePosition = new Vector2Int(horizontalPosition, verticalPosition);
                if (managerRunes.btnNextRoom != null)
                {
                    managerRunes.btnNextRoom.SetActive(true);
                }                
                managerRunes.prefabsRunes.Add(rune.gameObject);
                managerRunes.allRunes.Add(rune);
                rune.isUsed = true;
                managerRunes.DesactiveteOptions();
                RuneClass runeClass = new RuneClass()
                {
                    runeName = rune.runeName,
                    level = rune.level,
                    savePosition = rune.savePosition,
                    cost = rune.cost,
                };
                managerRunes.runesDungeon.Add(runeClass);
            }
            
            rune.transform.position = transform.position;
            rune.transform.localScale = Vector3.one;
            rune.transform.parent = managerRunes.runePanel.transform;
            foreach (var slot in rune.positionsSlots)
            {
                managerRunes.slotsChecker[horizontalPosition + slot.x, verticalPosition + slot.y] = false;
            }
            int tutorialRunes = PlayerPrefs.GetInt("TutorialRunes", 0);
            if (tutorialRunes == 0)
            {
                managerRunes.DisabledChat.Invoke();
                PlayerPrefs.SetInt("TutorialRunes", 1);
            }
        }      
    }

    public void PositiveCheck()
    {
        
        canDrop = true;
        
        Debug.Log("PositiveCheck");
    }

    public void NegativeCheck()
    {
        canDrop = false;
    }

    private bool ValidatePosition(int x, int y, Vector2Int slot)
    {
        int newX = x + slot.x;
        int newY = y + slot.y;

        // Verificar que la nueva posición está dentro de los límites
        return newX >= 0 && newX < 4 && newY >= 0 && newY < 8;
    }
}
