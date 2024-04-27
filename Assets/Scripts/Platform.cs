using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private float platformSpeed;

    const string PLAYERTAG = "Player";

    void FixedUpdate()
    {
        transform.Translate(new Vector3(0, -platformSpeed * Time.fixedDeltaTime, 0));
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(PLAYERTAG))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(PLAYERTAG))
        {
            collision.transform.SetParent(null);
        }
    }
}
