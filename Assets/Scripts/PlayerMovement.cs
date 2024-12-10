using UnityEngine;
using System.Collections;
public class PlayerMovement : MonoBehaviour
{
    #region KeyCode
    KeyCode JumpButton;
    KeyCode CrouchButton;
    KeyCode LeftButton;
    KeyCode RightButton;
    #endregion

    #region string
    const string Left = "Left", Right = "Right", Forward = "Forward";
    #endregion

    #region floats
    [SerializeField] float speed = 5;
    [SerializeField] float RLspeed;
    [SerializeField] float HitCooldown;
    [SerializeField] float jumpForce = 400f;
    [SerializeField] float DistanceToGround;
    [SerializeField] float laneDistance = 4;//the distance between 2 lanes
    #endregion

    #region Int
    #region ChangeLine
    private int desiredlane = 1; //0:left,1:middle,2:right
    private int previousdesiredlane = 1;


    #endregion
    #endregion

    #region Constructer
    [SerializeField] Rigidbody rb;
    private Animator animator;
    [SerializeField] LayerMask groundMask;
    Vector3 forwardMove;
    #endregion

    #region Booleans
    [HideInInspector] public bool isInvert = false;

    private bool isJumping = false;
    [HideInInspector] public bool CanMove = true;
    private bool IsHit;
    [HideInInspector] public bool isDead;
    [SerializeField] bool isGrounded;
    #endregion


    private void FixedUpdate()
    {
        if (CanMove)
            ChangeLines();
        else
            return;
    }


    // CHIMONEY CODE

    private void Start()
    {
        animator = GetComponent<Animator>();
    }



    private void Update()
    {
        if (CanMove)
        {
            float height = GetComponent<Collider>().bounds.size.y;
            isGrounded = Physics.Raycast(transform.position, Vector3.down, DistanceToGround);


            //jumping
            if (Input.GetKeyDown(JumpButton) == true && isGrounded == true && animator.GetCurrentAnimatorStateInfo(0).IsName("Land") == false)
            {

                Jump();

            }
            if (Input.GetKeyDown(CrouchButton) && animator.GetCurrentAnimatorStateInfo(0).IsName("Slide") == false)
            {
                animator.SetTrigger("Slide");
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
                animator.ResetTrigger("Slide");
            if (Mathf.Abs(rb.velocity.y) < 0.001f)
                isJumping = false;

            CheckAnimator();
            CheckLines();
        }
        else
            Debug.Log("Dead");

        if (!isInvert)
        {
            JumpButton = KeyCode.Space;
            CrouchButton = KeyCode.LeftControl;
            RightButton = KeyCode.D;
            LeftButton = KeyCode.A;
        }
        else
        {
            JumpButton = KeyCode.LeftControl;
            CrouchButton = KeyCode.Space;
            RightButton = KeyCode.A;
            LeftButton = KeyCode.D;
        }


    }
    private void ApplyJumpUpForce()
    {
        rb.AddRelativeForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }
    private void CheckLines()
    {
        //Debug.Log(previousdesiredlane+ "previousdesiredlane");
        //Debug.Log(desiredlane + "desiredlane");
        //MoveLeftAndRight
        if (Input.GetKeyDown(RightButton))
        {
            previousdesiredlane = 0;
            previousdesiredlane = desiredlane;
            desiredlane++;
            if (desiredlane == 3)
                desiredlane = 2;

        }

        if (Input.GetKeyDown(LeftButton))
        {
            previousdesiredlane =  0;
            previousdesiredlane = desiredlane;
            desiredlane--;
            if (desiredlane == -1)
                desiredlane = 0;


        }
    }
    private void Died()
    {
        isDead = true;
        CanMove = false;

        animator.SetTrigger("Death");
    }
    private void CheckAnimator()
    {
        if (rb.velocity.y <= 0 && rb.velocity.y > -0.5f)
        {
            animator.SetBool("Jumping", false);
            animator.SetBool("Landing", false);
        }
        if (rb.velocity.y > 0.01f)
        {
            animator.SetBool("Jumping", true);
        }

        if (rb.velocity.y < -1.8)
        {
            animator.SetBool("Jumping", false);

            animator.SetBool("Landing", true);
        }
        if (rb.velocity.y >= 0)
            animator.SetBool("Landing", false);

    

        if (IsHit == true)       
            animator.SetBool("WalkHit", true);
        else
            animator.SetBool("WalkHit", false);



    }
    private void ChangeLines()
    {
        Vector3 TargetPostion = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredlane == 0)
        {

            TargetPostion += Vector3.left * laneDistance;
        }
        else if (desiredlane == 2)
        {

            TargetPostion += Vector3.right * laneDistance;
        }
        TargetPostion += Vector3.forward * speed;
        transform.position = Vector3.Lerp(transform.position, TargetPostion, RLspeed * Time.deltaTime);


    }
    void Jump() => rb.AddForce(Vector3.up * jumpForce);

    private void OnCollisionEnter(Collision collision)
    {
        if (isJumping == true&&collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            animator.SetTrigger("Landing");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == Forward)
        {
            Died();
        }else if (other.gameObject.name == Left)
        {
            if (IsHit==true && isDead == false)
            {
                Died();
            }
            else
            {
                Debug.Log("Left");
                StartCoroutine(HitCooldownEnum());
                   IsHit = true;
                if (previousdesiredlane == 1)
                {
                    desiredlane = 1;
                }else if (previousdesiredlane == 0)
                {
                    desiredlane = 0;
                }
            }
        }
        else if (other.gameObject.name == Right)
        {
            if (IsHit == true&&isDead==false)
            {
                Died();
            }
            else
            {
                Debug.Log("Right");
                StartCoroutine(HitCooldownEnum());
                IsHit = true;
                if (previousdesiredlane == 2)
                {
                    desiredlane = 2;
                }
                else if (previousdesiredlane == 1)
                {
                    desiredlane = 1;
                }
            }
        }
        //else if (other.gameObject.name == Left)
        //{
        //    if (!IsHit)
        //    {
        //        Died();
        //    }
        //    else
        //    {
        //        if (previousdesiredlane == 3)
        //        {
        //            desiredlane = 2;
        //        }
        //    }
        //}

    }
    IEnumerator HitCooldownEnum()
    {
        IsHit = true;
       yield return new WaitForSeconds(HitCooldown);
        IsHit = false;
            
    }
}
