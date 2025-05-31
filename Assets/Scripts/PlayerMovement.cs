using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f; // grados por segundo
    public bool canMove = true;

    private Vector3 moveDirection;

    void Update()
    {
        // Obtener entrada
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D o Flechas Izq/Der
        float vertical = Input.GetAxisRaw("Vertical");     // W/S o Flechas Arr/Ab

        // Crear dirección de movimiento
        if (canMove)
        {
            moveDirection = new Vector3(-vertical, 0f, horizontal).normalized;

            // Mover al personaje
            if (moveDirection.magnitude > 0)
            {
                transform.position += moveDirection * moveSpeed * Time.deltaTime;

                // Rotar hacia la dirección del movimiento
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }       
    }

    public void DesactiveMovement()
    {
        canMove = false;
    }
}
