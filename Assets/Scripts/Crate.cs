using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] Transform crateScoreSpawn;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] SpriteRenderer highlightSprite;

    [SerializeField] private float fallingSpeed;

    private Team crateId;

    private float knockbackForce;
    private float attackPoints = 0;

    const string PLAYERTAG = "Player";
    const string THROWN_CRATE_TAG = "ThrownCrate";

    void Start()
    {
        highlightSprite.color = LevelManager.colorDict[crateId];
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        rb.velocity = Vector2.down * fallingSpeed;

        if (transform.position.x > 0)
        {
            crateId = Team.right;
            highlightSprite.color = LevelManager.colorDict[Team.right];
        }
        else
        {
            crateId = Team.left;
            highlightSprite.color = LevelManager.colorDict[Team.left];
        }
    }

    private void OnBecameVisible()
    {
        // don't let the player interact with a crate he cannot see
        boxCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PLAYERTAG))
        {
            if(collision.GetComponent<Player>().GetPlayerId() == crateId)
            {
                highlightSprite.enabled = true;
            }
            else if (tag == THROWN_CRATE_TAG)
            {
                Rigidbody2D playerBody = collision.gameObject.GetComponent<Rigidbody2D>();
                playerBody.AddForce(Vector2.right * transform.localScale.x * knockbackForce, ForceMode2D.Impulse);

                LevelManager.Singleton.SetPlayerScore(crateId, attackPoints);

                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(PLAYERTAG))
        {
            highlightSprite.enabled = false;
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public Team GetCrateId()
    {
        return crateId;
    }

    public void ShootCrate(Vector2 attackForce)
    {
        tag = THROWN_CRATE_TAG;
        rb.AddForce(attackForce, ForceMode2D.Impulse);
        boxCollider.size = Vector2.one;
    }

    public void SetKnockbackForce(float knockbackForce)
    {
        this.knockbackForce = knockbackForce;
    }

    public void SetAttackPoint(float attackPoints)
    {
        this.attackPoints = attackPoints;
    }

    public Vector2 GetScorePosition()
    {
        return crateScoreSpawn.position;
    }
}
