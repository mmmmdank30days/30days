using UnityEngine;

public class DigTool : MonoBehaviour
{
    public float digRange = 16f;
    public KeyCode digKey = KeyCode.E;
    public LayerMask diggableLayer;
    public GameObject digEffect;
    public Transform handTransform; // Assign this to the left hand or shovel in Inspector
    public Material highlightMaterial;

    private GameObject currentHighlighted;
    private Material originalMaterial;

    private Inventory inventory;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            inventory = player.GetComponent<Inventory>();
        }
    }

    void Update()
    {
        if (handTransform == null)
        {
            Debug.LogWarning("❗ handTransform not assigned.");
            return;
        }

        Vector3 origin = handTransform.position;
        Vector3 direction = (handTransform.forward + Vector3.down * 2.5f).normalized;
        Debug.DrawRay(origin, direction * digRange, Color.red, 0.2f);

        Ray ray = new Ray(origin, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, digRange, diggableLayer))
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

                if (Input.GetKeyDown(digKey))
                {
                    Debug.Log("⛏️ Dug into: " + hitObject.name);

                    if (digEffect != null)
                        Instantiate(digEffect, hit.point, Quaternion.identity);

                    Destroy(hitObject);

                    if (inventory != null)
                    {
                        inventory.AddItem("Dirt", 1);
                    }

                    ClearHighlight();
                }
            }
            else
            {
                ClearHighlight();
            }
        }
        else
        {
            ClearHighlight();
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
