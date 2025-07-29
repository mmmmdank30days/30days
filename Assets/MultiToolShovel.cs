using UnityEngine;

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
    public Camera playerCamera; // 🔧 Assign Main Camera in Inspector

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
        if (Input.GetKeyDown(toggleModeKey))
        {
            currentMode = currentMode == Mode.Dig ? Mode.Place : Mode.Dig;
            ClearHighlight(); // 🧹 Clear highlight when switching modes
            Debug.Log("🔁 Switched to " + currentMode + " mode");
        }

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); // Center of screen
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red);

        if (currentMode == Mode.Dig)
            HandleDig(ray);
        else
            HandlePlace(ray);
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
                    Debug.Log("⛏️ Dug into: " + hitObject.name);
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
        {
            currentGhost = Instantiate(ghostPreview);
        }

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
