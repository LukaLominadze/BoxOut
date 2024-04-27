using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform mainCamera;
    [SerializeField] SpriteRenderer beltSprite;

    [SerializeField] Team playerId;
    [SerializeField] private float positionXLimit;

    private void Start()
    {
        mainCamera = Camera.main.transform;
        beltSprite.color = LevelManager.colorDict[playerId];
    }

    private void FixedUpdate()
    {
        if (playerId == Team.left)
        {
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, positionXLimit, -0.5f),
                                             Mathf.Clamp(transform.position.y, -Mathf.Infinity, mainCamera.position.y + 4));
        }
        else
        {
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, 0.5f, positionXLimit),
                                             Mathf.Clamp(transform.position.y, -Mathf.Infinity, mainCamera.position.y + 4));
        }
    }

    private void OnBecameInvisible()
    {
        LevelManager.Singleton.EndGame(playerId);
    }

    public Team GetPlayerId()
    {
        return playerId;
    }
}
