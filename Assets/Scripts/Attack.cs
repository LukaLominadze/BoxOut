using TMPro;
using UnityEngine;

public enum AttackState { initialize, rotate, attack, cooldown }

public class Attack : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoxCollider2D playerCollider;
    [SerializeField] Movement movementScript;
    [SerializeField] CatchCrate catchCrate;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private float attackForce;
    [SerializeField] private float knockbackForce = 0;
    [SerializeField] private float maxAttackPoints = 100;
    [SerializeField] private float minusPointsPerSec = 25;

    [SerializeField] private string axis;

    [SerializeField] private KeyCode attackKey;

    private GameObject currentCrate;

    private TextMeshProUGUI crateScore;

    private Crate crateScript;

    private AttackState attackState = AttackState.cooldown;

    private float direction;
    private float elapsedTime = 0;
    private float currentAttackPoints = 0;

    private void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        movementScript = GetComponent<Movement>();
        catchCrate = GetComponent<CatchCrate>();
    }

    private void Update()
    {
        direction = Input.GetAxisRaw(axis);
    }

    private void FixedUpdate()
    {
        switch (attackState)
        {
            case AttackState.initialize:
                crateScript = currentCrate.GetComponent<Crate>();
                currentCrate.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
                movementScript.enabled = false;

                crateScore = LevelManager.Singleton.SpawnCrateScore(crateScript.GetScorePosition());

                if (player.GetPlayerId() == Team.right)
                {
                    currentCrate.transform.rotation = Quaternion.Euler(0, 0, 180);
                    currentCrate.transform.localScale = new Vector3(-1, 1, 1);
                }

                transform.SetParent(null);
                playerCollider.isTrigger = true;

                ChangeAttackState(AttackState.rotate);
                break;
            case AttackState.rotate:
                currentCrate.transform.rotation *= Quaternion.Euler(0,
                    0,
                    (currentCrate.transform.localScale.x * direction * rotationSpeed) * Time.fixedDeltaTime);

                elapsedTime += Time.fixedDeltaTime;

                currentAttackPoints = Mathf.Round(maxAttackPoints - (elapsedTime * minusPointsPerSec));
                currentAttackPoints = Mathf.Clamp(currentAttackPoints, 0, maxAttackPoints);

                crateScore.SetText($"{currentAttackPoints}");

                if (Input.GetKeyDown(attackKey))
                {
                    elapsedTime = 0;

                    StartCoroutine(LevelManager.Singleton.DestroyCrateScore(crateScore.gameObject, 1));

                    ChangeAttackState(AttackState.attack);
                }
                break;
            case AttackState.attack:
                Physics2D.IgnoreCollision(currentCrate.GetComponent<BoxCollider2D>(), playerCollider);

                crateScript.ShootCrate(currentCrate.transform.right * attackForce);
                crateScript.SetAttackPoint(currentAttackPoints);
                //crateScript.SetKnockbackForce(knockbackForce);

                catchCrate.LetGoOffCrate();

                rb.gravityScale = 1;
                movementScript.enabled = true;

                playerCollider.isTrigger = false;

                ChangeAttackState(AttackState.cooldown);
                break;
            case AttackState.cooldown:
                //we wait for the catch crate script
                break;
        }

        if (currentCrate == null && attackState != AttackState.cooldown)
        {
            ChangeAttackState(AttackState.cooldown);
        }
    }

    public AttackState GetAttackState()
    {
        return attackState;
    }

    public void ChangeAttackState(AttackState newState)
    {
        attackState = newState;
    }

    public GameObject GetCrate()
    {
        return currentCrate;
    }

    public void SetCrate(GameObject crate)
    {
        currentCrate = crate;
    }
}
