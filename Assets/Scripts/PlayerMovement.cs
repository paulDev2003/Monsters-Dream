using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f; // grados por segundo
    public bool canMove = true;
    public Rigidbody rb;
    private Vector3 moveDirection;
    public float speed;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (canMove)
        {
            moveDirection = new Vector3(-vertical, 0f, horizontal).normalized;

            if (moveDirection.magnitude > 0)
            {
                rb.linearVelocity = moveDirection * moveSpeed;

                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                rb.linearVelocity = Vector3.zero;
            }
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    public void ActiveMovement()
    {
        canMove = true;
    }

    public void DesactiveMovement()
    {
        canMove = false;
    }
}
