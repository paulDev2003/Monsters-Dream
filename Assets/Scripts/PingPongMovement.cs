using UnityEngine;

public class PingPongMovement : MonoBehaviour
{
    public float moveDistance = 0.5f; // Qué tan lejos se mueve
    public float moveSpeed = 1.0f;    // Velocidad del movimiento
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance * 2) - moveDistance;
        transform.position = startPosition + new Vector3(0, offset, 0); // Mueve verticalmente
    }
}
