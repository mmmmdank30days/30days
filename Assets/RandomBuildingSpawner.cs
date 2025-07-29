using UnityEngine;

public class RandomBuildingSpawner : MonoBehaviour
{
    public GameObject buildingPrefabA;
    public GameObject buildingPrefabB;
    public int buildingCount = 20;

    public Vector2 spawnXRange = new Vector2(0f, 100f);
    public Vector2 spawnZRange = new Vector2(0f, 100f);
    public Vector2 spawnYRange = new Vector2(10f, 150f);

    public float collisionCheckRadius = 5f;
    public LayerMask groundLayer;
    public LayerMask collisionLayer;

    private int placedA = 0;
    private int placedB = 0;

    void Start()
    {
        SpawnBuildings();
    }

    void SpawnBuildings()
    {
        int attempts = 0;
        int placed = 0;
        int maxAttempts = buildingCount * 10;

        while (placed < buildingCount && attempts < maxAttempts)
        {
            attempts++;

            Vector3 rayOrigin = new Vector3(
                Random.Range(spawnXRange.x, spawnXRange.y),
                spawnYRange.y,
                Random.Range(spawnZRange.x, spawnZRange.y)
            );

            // Raycast down to ground
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, spawnYRange.y - spawnYRange.x, groundLayer))
            {
                Vector3 groundTop = hit.point;

                // Collision check
                if (Physics.CheckSphere(groundTop, collisionCheckRadius, collisionLayer))
                    continue;

                GameObject prefabToUse;

                // Alternate prefabs for balance
                if (placedA <= placedB)
                {
                    prefabToUse = buildingPrefabA;
                    placedA++;
                }
                else
                {
                    prefabToUse = buildingPrefabB;
                    placedB++;
                }

                GameObject building = Instantiate(prefabToUse);
                Renderer rend = building.GetComponentInChildren<Renderer>();
                if (rend != null)
                {
                    float bottomY = rend.bounds.min.y;
                    float offsetY = groundTop.y - bottomY;
                    building.transform.position = new Vector3(groundTop.x, building.transform.position.y + offsetY, groundTop.z);
                }
                else
                {
                    building.transform.position = groundTop;
                }

                building.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                placed++;
            }
        }

        Debug.Log($"✅ Spawned {placed} buildings ({placedA} A, {placedB} B) after {attempts} attempts.");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = new Vector3(
            (spawnXRange.x + spawnXRange.y) / 2f,
            (spawnYRange.x + spawnYRange.y) / 2f,
            (spawnZRange.x + spawnZRange.y) / 2f
        );

        Vector3 size = new Vector3(
            Mathf.Abs(spawnXRange.y - spawnXRange.x),
            Mathf.Abs(spawnYRange.y - spawnYRange.x),
            Mathf.Abs(spawnZRange.y - spawnZRange.x)
        );

        Gizmos.DrawWireCube(center, size);
    }
}
