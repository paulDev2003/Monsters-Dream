using UnityEngine;

public class LookToCamera : MonoBehaviour
{

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
