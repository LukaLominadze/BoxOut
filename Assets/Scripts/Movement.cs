using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerCollision collision;

    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float accelSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallAccel;
    [SerializeField] private float fastFallAccel;
    [SerializeField] private float onGroundVel;

    [SerializeField] private string axis;

    [SerializeField] private KeyCode jumpKey;

    private float direction;
    private float gravity;

    private bool jumpInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collision = rb.GetComponent<PlayerCollision>();

        gravity = Physics2D.gravity.y;
    }

    void Update()
    {
        direction = Input.GetAxis(axis);
        jumpInput = Input.GetKey(jumpKey);
    }

    private void FixedUpdate()
    {
        float currentSpeed = rb.velocity.x;
        //desired speed
        float desiredSpeed = direction * speed;
        //difference between current speed and desired speed
        float speedDiffernce = desiredSpeed - currentSpeed;
        //if desired speed is > 0 than we're gonna accelerate, otherwise, deccelerate
        float accelRate = (Mathf.Abs(desiredSpeed) > 0.01f) ? acceleration : decceleration;
        //if we have not reached desired speed, we are going to accelerate(or deccelerate)
        float movement = Mathf.Pow(Mathf.Abs(speedDiffernce) * accelRate, accelSpeed) * Mathf.Sign(speedDiffernce);

        rb.AddForce(Vector2.right * movement);

        if (collision.OnGround() && rb.velocity.y < 0.01f)
        {
            if (jumpInput)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        if (rb.velocity.y > 0.01f && !jumpInput)
        {
            rb.velocity += Vector2.up * gravity * fastFallAccel * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y < 0.01f)
        {
            rb.velocity += Vector2.up * gravity * fallAccel * Time.fixedDeltaTime;
        }
    }
}
