using UnityEngine;

public class PlaceTool : MonoBehaviour
{
    public GameObject dirtPrefab;
    public GameObject ghostPrefab;
    public float placeDistance = 3f;
    public KeyCode placeKey = KeyCode.R;
    public Transform handTransform; // 👈 NEW — Assign this to left hand or tool in Inspector

    private Inventory inventory;
    private GameObject ghostInstance;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            inventory = player.GetComponent<Inventory>();

        // Create ghost instance
        if (ghostPrefab != null)
        {
            ghostInstance = Instantiate(ghostPrefab);
            ghostInstance.SetActive(false);
        }
    }

    void Update()
    {
        if (inventory == null || ghostInstance == null || handTransform == null)
            return;

        // Aim direction from hand, angled downward-forward
        Vector3 origin = handTransform.position;
        Vector3 direction = (handTransform.forward).normalized;
        Vector3 targetPosition = origin + direction * placeDistance;

        // Snap to grid
        Vector3 snappedPosition = SnapToGrid(targetPosition);

        // Ghost preview
        if (inventory.HasItem("Dirt", 1))
        {
            ghostInstance.SetActive(true);
            ghostInstance.transform.position = snappedPosition;
        }
        else
        {
            ghostInstance.SetActive(false);
        }

        // Place block
        if (Input.GetKeyDown(placeKey) && inventory.HasItem("Dirt", 1))
        {
            Instantiate(dirtPrefab, snappedPosition, Quaternion.identity);
            inventory.RemoveItem("Dirt", 1);
            Debug.Log("🧱 Placed dirt at " + snappedPosition);
        }
    }

    Vector3 SnapToGrid(Vector3 pos)
    {
        return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
    }
}
