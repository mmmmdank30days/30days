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

    void Update()
    {
        if (handTransform == null)
        {
            Debug.LogWarning("❗ handTransform not assigned.");
            return;
        }

        // Always raycast to check what player is aiming at
        Vector3 origin = handTransform.position;
        Vector3 direction = (handTransform.forward + Vector3.down * 2.5f).normalized;

        Debug.DrawRay(origin, direction * digRange, Color.red, 0.2f);

        Ray ray = new Ray(origin, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, digRange, diggableLayer))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Highlight logic
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

                // If dig key is pressed and target is valid
                if (Input.GetKeyDown(digKey))
                {
                    Debug.Log("⛏️ Dug into: " + hitObject.name);

                    if (digEffect != null)
                        Instantiate(digEffect, hit.point, Quaternion.identity);

                    Destroy(hitObject);
                    ClearHighlight(); // Clean up reference after dig
                }
            }
            else
            {
                ClearHighlight(); // It's not diggable
            }
        }
        else
        {
            ClearHighlight(); // Nothing hit
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
