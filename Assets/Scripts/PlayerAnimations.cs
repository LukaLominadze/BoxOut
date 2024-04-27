using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerCollision collision;

    [SerializeField] private string axis;

    private string currentState = "";

    const string PLAYERIDLE = "PlayerIdle";
    const string PLAYERWALK = "PlayerWalk";
    const string PLAYERJUMP = "PlayerJump";
    const string PLAYERFALL = "PlayerFall";
    const string PLAYERCATCH = "PlayerCatch";

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collision = GetComponent<PlayerCollision>();

        currentState = PLAYERIDLE;
    }

    void Update()
    {
        float direction = Input.GetAxisRaw(axis);

        if (direction != 0)
        {
            transform.localScale = new Vector3(direction, 1, 1);
        }

        if (rb.gravityScale == 0)
        {
            ChangeAnimationState(PLAYERCATCH);
            return;
        }
        if (collision.OnGround())
        {
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                ChangeAnimationState(PLAYERIDLE);
            }
            else
            {
                ChangeAnimationState(PLAYERWALK);
            }
        }
        else
        {
            if (rb.velocity.y > 0.01f)
            {
                ChangeAnimationState(PLAYERJUMP);
            }
            else if (rb.velocity.y < 0.01f)
            {
                ChangeAnimationState(PLAYERFALL);
            }
        }
    }

    private void ChangeAnimationState(string newState)
    {
        if (newState == currentState) return;
        animator.Play(newState);
        currentState = newState;
    }
}
