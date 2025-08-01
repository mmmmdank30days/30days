using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public Vector2 spawnXRange = new Vector2(0f, 100f);
    public Vector2 spawnZRange = new Vector2(0f, 100f);
    public Vector2 spawnHeightRange = new Vector2(5f, 150f);
    public Vector2 spawnScaleRange = new Vector2(3f, 50f);

    private GameObject spawnedAsteroid;

    void Start()
    {
        SpawnRandomAsteroid();
    }

    void SpawnRandomAsteroid()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnXRange.x, spawnXRange.y),
            Random.Range(spawnHeightRange.x, spawnHeightRange.y),
            Random.Range(spawnZRange.x, spawnZRange.y)
        );

        float randomScale = Random.Range(spawnScaleRange.x, spawnScaleRange.y);

        spawnedAsteroid = Instantiate(asteroidPrefab, randomPosition, Quaternion.identity);
        spawnedAsteroid.transform.localScale = Vector3.one * randomScale;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 center = new Vector3(
            (spawnXRange.x + spawnXRange.y) / 2f,
            (spawnHeightRange.x + spawnHeightRange.y) / 2f,
            (spawnZRange.x + spawnZRange.y) / 2f
        );

        Vector3 size = new Vector3(
            Mathf.Abs(spawnXRange.y - spawnXRange.x),
            Mathf.Abs(spawnHeightRange.y - spawnHeightRange.x),
            Mathf.Abs(spawnZRange.y - spawnZRange.x)
        );

        Gizmos.DrawWireCube(center, size);
    }
}
