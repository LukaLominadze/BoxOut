using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] LayerMask groundLayer;

    [SerializeField] private Vector2 size;

    [SerializeField] private float offset;

    private bool onGround;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public bool OnGround()
    {
        if (rb.velocity.y > 0.01f)
        {
            return false;
        }
        onGround = Physics2D.BoxCast(transform.position, size, 0, Vector2.down, offset, groundLayer);
        return onGround;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawCube(new Vector2(transform.position.x, transform.position.y - offset), size);
    }
}
