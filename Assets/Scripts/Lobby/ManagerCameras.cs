using UnityEngine;
using Unity.Cinemachine;

public class ManagerCameras : MonoBehaviour
{
    public CinemachineCamera currentCamera;

    public void ChangeCamera(CinemachineCamera targetCam)
    {
        targetCam.Priority = 10;
        currentCamera.Priority = 5;
        currentCamera = targetCam;
    }
}
