using UnityEngine;

public class BarrelMove : MonoBehaviour
{
    public float speed = -5;
    public Rigidbody rb;

    private void FixedUpdate()
    {
        Vector3 forwardMove = transform.right * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove);
        
    }

    // CHIMONEY CODE


}
