using UnityEngine;

public class BtnEggProgress : MonoBehaviour
{
    public ItemSO savedItem;
    public EggPanel eggPanel;
    public int total;
    public int current = 0;
    public int valueI;

    public void AddProgress()
    {
        eggPanel.AddProgress(this);
    }
}
