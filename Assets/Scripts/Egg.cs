using UnityEngine;

public class Egg : MonoBehaviour
{
    public EggSO eggSO;
    public Bestiary bestiary;
    public bool growing = false;
    public EggData eggData;
    public EggSpot eggSpot;


    private void OnMouseDown()
    {
        if (growing)
        {
            bestiary.eggInvoked = this;
            bestiary.ShowEggPanel.Invoke();
        }
    }
}
