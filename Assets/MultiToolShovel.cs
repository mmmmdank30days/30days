using UnityEngine;
using UnityEngine.Rendering.Universal; // Needed for DecalProjector

public class MultiTool : MonoBehaviour
{
    public float range = 8f;
    public KeyCode actionKey = KeyCode.E;
    public KeyCode toggleModeKey = KeyCode.Q;
    public LayerMask diggableLayer;
    public GameObject digEffect;
    public GameObject dirtPrefab;
    public GameObject ghostPreview;
    public Material highlightMaterial;
    public Camera playerCamera;

    public Terrain terrain;
    public float digSize = 3f;
    public float digDepth = 0.01f;

    public DecalProjector digDecalPreviewPrefab; // Assign your URP decal prefab in Inspector
    private DecalProjector currentDecal;

    private enum Mode { Dig, Place }
    private Mode currentMode = Mode.Dig;

    private GameObject currentHighlighted;
    private Material originalMaterial;
    private GameObject currentGhost;
    private Inventory inventory;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            inventory = player.GetComponent<Inventory>();
    }

    void Update()
    {
        if (!GameState.ControlsEnabled) return;

        if (Input.GetKeyDown(toggleModeKey))
        {
            currentMode = currentMode == Mode.Dig ? Mode.Place : Mode.Dig;
            ClearHighlight();
            RemoveDecal(); // 🧼 Clear decal when switching modes
            Debug.Log("🔁 Switched to " + currentMode + " mode");
        }

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red);

        if (currentMode == Mode.Dig)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, range))
            {
                if (hit.collider.GetComponent<Terrain>())
                {
                    UpdateDecalPosition(hit.point);
                    if (Input.GetKeyDown(actionKey))
                    {
                        HandleDigTerrain(hit.point);
                    }
                }
                else
                {
                    ClearHighlight();
                    RemoveDecal();
                    HandleDig(ray);
                }
            }
            else
            {
                ClearHighlight();
                RemoveDecal();
            }
        }
        else
        {
            RemoveDecal();
            HandlePlace(ray);
        }
    }

    void HandleDigTerrain(Vector3 point)
    {
        TerrainData tData = terrain.terrainData;
        Vector3 terrainPos = point - terrain.transform.position;

        int x = Mathf.FloorToInt((terrainPos.x / tData.size.x) * tData.heightmapResolution);
        int z = Mathf.FloorToInt((terrainPos.z / tData.size.z) * tData.heightmapResolution);
        int digRadius = Mathf.FloorToInt((digSize / tData.size.x) * tData.heightmapResolution);

        float[,] heights = tData.GetHeights(x, z, digRadius, digRadius);
        for (int i = 0; i < digRadius; i++)
        {
            for (int j = 0; j < digRadius; j++)
            {
                heights[i, j] -= digDepth;
                heights[i, j] = Mathf.Clamp01(heights[i, j]);
            }
        }
        tData.SetHeights(x, z, heights);

        if (inventory != null)
            inventory.AddItem("Dirt", 1);

        if (digEffect != null)
            Instantiate(digEffect, point, Quaternion.identity);
    }

    void UpdateDecalPosition(Vector3 point)
    {
        if (digDecalPreviewPrefab == null) return;

        if (currentDecal == null)
        {
            currentDecal = Instantiate(digDecalPreviewPrefab);
        }

        currentDecal.transform.position = new Vector3(point.x, point.y + 0.1f, point.z);
        currentDecal.transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Point downward
    }

    void RemoveDecal()
    {
        if (currentDecal != null)
        {
            Destroy(currentDecal.gameObject);
            currentDecal = null;
        }
    }

    void HandleDig(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, range, diggableLayer))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag("Diggable"))
            {
                if (currentHighlighted != hitObject)
                {
                    ClearHighlight();
                    currentHighlighted = hitObject;
                    Renderer rend = currentHighlighted.GetComponent<Renderer>();
                    if (rend != null)
                    {
                        originalMaterial = rend.material;
                        rend.material = highlightMaterial;
                    }
                }

                if (Input.GetKeyDown(actionKey))
                {
                    if (digEffect != null)
                        Instantiate(digEffect, hit.point, Quaternion.identity);

                    Destroy(hitObject);
                    if (inventory != null)
                        inventory.AddItem("Dirt", 1);

                    ClearHighlight();
                }
            }
        }
        else
        {
            ClearHighlight();
        }
    }

    void HandlePlace(Ray ray)
    {
        if (currentGhost == null)
            currentGhost = Instantiate(ghostPreview);

        Vector3 targetPos = ray.origin + ray.direction * range;
        Vector3 snappedPos = new Vector3(
            Mathf.Round(targetPos.x),
            Mathf.Round(targetPos.y),
            Mathf.Round(targetPos.z)
        );
        currentGhost.transform.position = snappedPos;

        if (Input.GetKeyDown(actionKey) && inventory != null && inventory.HasItem("Dirt"))
        {
            Instantiate(dirtPrefab, snappedPos, Quaternion.identity);
            inventory.RemoveItem("Dirt", 1);
        }
    }

    void ClearHighlight()
    {
        if (currentHighlighted != null)
        {
            Renderer rend = currentHighlighted.GetComponent<Renderer>();
            if (rend != null && originalMaterial != null)
                rend.material = originalMaterial;

            currentHighlighted = null;
            originalMaterial = null;
        }
    }
}
