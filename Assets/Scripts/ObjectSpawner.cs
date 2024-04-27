using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject objectPrefab;

    [SerializeField] Transform boundaryLeft;
    [SerializeField] Transform boundaryRight;

    [SerializeField] private float spawnPosY;

    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;

    private float currentSpawnTime;

    private float elapsedTime = 0f;

    void FixedUpdate()
    {
        elapsedTime += Time.fixedDeltaTime;
        if (elapsedTime > currentSpawnTime)
        {
            Instantiate(objectPrefab,
                        new Vector2(Random.Range(boundaryLeft.position.x, boundaryRight.position.x),
                                                 boundaryLeft.position.y),
                        Quaternion.identity);
            SetSpawnTime();
        }
    }

    private void SetSpawnTime()
    {
        currentSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        elapsedTime = 0;
    }
}
