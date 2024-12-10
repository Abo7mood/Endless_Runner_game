using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = -5;
    public Rigidbody rb;

    private void FixedUpdate()
    {
        Vector3 forwardMove = -transform.forward * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove);
    }

    // CHIMONEY CODE

  
}
