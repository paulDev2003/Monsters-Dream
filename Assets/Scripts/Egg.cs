using UnityEngine;
using UnityEngine.EventSystems;

public class Egg : MonoBehaviour
{
    public EggSO eggSO;
    public ManagerEggSpots managerEggSpots;
    public bool growing = false;
    public EggData eggData;
    public EggSpot eggSpot;


    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (growing)
        {
            managerEggSpots.eggInvoked = this;
            managerEggSpots.ShowEggPanel.Invoke();
            managerEggSpots.DesactiveUI.Invoke();
        }
    }
}
