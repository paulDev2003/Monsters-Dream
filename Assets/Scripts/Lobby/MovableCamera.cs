using UnityEngine;
using Unity.Cinemachine;

public class MovableCamera : MonoBehaviour
{
    public CinemachineCamera cam;
    public ManagerCameras managerCameras;

    public void WatchCamera()
    {
        managerCameras.ChangeCamera(cam);
    }
}
