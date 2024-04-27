using UnityEngine;

public class CatchCrate : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Attack attackScript;

    [SerializeField] private KeyCode catchKey;

    private bool caughtCrate = false;

    const string CRATE_TAG = "Crate";

    private void Start()
    {
        player = GetComponent<Player>();
        attackScript = GetComponent<Attack>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (caughtCrate) return;

        if (collision.CompareTag(CRATE_TAG))
        {
            if(collision.gameObject.GetComponent<Crate>().GetCrateId() != player.GetPlayerId())
            {
                return;
            }

            if (attackScript.GetCrate() == null)
            {
                attackScript.SetCrate(collision.gameObject);
            }
            else
            {
                if (Input.GetKeyDown(catchKey))
                {
                    attackScript.ChangeAttackState(AttackState.initialize);
                    caughtCrate = true;
                }
            }
        }
    }

    public void LetGoOffCrate()
    {
        caughtCrate = false;
    }
}
