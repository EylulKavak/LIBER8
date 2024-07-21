using UnityEngine;

public class Movement : MonoBehaviour
{
    public LayerMask groundCheck;
    public float walkSpeed;
    public float runSpeed;
    public float smoothFactor;
    public float gravity;

    [HideInInspector] public float vertical, horizontal;
    float velocityHorizontal;
    float speed;

    Vector3 velocity = Vector3.zero;

    CharacterController charactercontroller;

    private void OnDrawGizmos()
    {
        if (GroundCheck()) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;

        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - transform.localScale.y * .8f, transform.position.z), transform.localScale.y * .5f);
    }
    private void Awake()
    {
        charactercontroller = GetComponent<CharacterController>();
        speed = walkSpeed;
    }
    private void FixedUpdate()
    {
        MovementAndAnim(horizontal, vertical);
    }
    public void MovementAndAnim(float h, float v)
    {
        if (!GroundCheck())
        {
            velocityHorizontal -= gravity;
        }
        velocity = Vector3.Lerp(velocity, (h * speed * transform.right + v * speed * transform.forward), smoothFactor * Time.deltaTime);
        charactercontroller.Move(Time.deltaTime * velocity + Time.deltaTime * velocityHorizontal * Vector3.up);
        Quaternion look = Camera.main.transform.rotation;
        look.x = 0;
        look.z = 0;
        transform.rotation = look;
    }
    public void RunPerformed()
    {
        if (GroundCheck())
        {
            speed = runSpeed;
        }
    }
    public void RunCancled()
    {
        speed = walkSpeed;
    }
    bool GroundCheck() => Physics.CheckSphere(transform.position - new Vector3(0, transform.localScale.y * .8f, 0), transform.localScale.y * .5f, groundCheck, QueryTriggerInteraction.Ignore);
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnDisable()
    {

        Cursor.lockState = CursorLockMode.None;
    }
}
