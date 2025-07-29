using UnityEngine;

public class DigTool : MonoBehaviour
{
    public float digRange = 20f; // How far you can dig
    public KeyCode digKey = KeyCode.E;
    public LayerMask diggableLayer;
    public GameObject digEffect;
    public Transform handTransform; // Assign this in the Inspector to the left hand or shovel

    void Update()
    {
        if (Input.GetKeyDown(digKey))
        {
            Debug.Log("hit key!");
            TryDig();
        }
    }

    void TryDig()
    {
        Vector3 rayOrigin = handTransform.position + Vector3.down * 1.5f;
        Vector3 rayDirection = (handTransform.forward + Vector3.down).normalized;
        Debug.DrawRay(rayOrigin, rayDirection * digRange, Color.red, 2000f);

        Ray ray = new Ray(rayOrigin, rayDirection);
        if (Physics.Raycast(ray, out RaycastHit hit, digRange, diggableLayer))
        {
            GameObject hitObject = hit.collider.gameObject;
            Debug.Log("✅ Hit something: " + hit.collider.name + " dd " + hitObject.tag);

            if (hitObject.CompareTag("Diggable"))
            {
                Debug.Log("⛏️ Dug into: " + hitObject.name);

                if (digEffect != null)
                    Instantiate(digEffect, hit.point, Quaternion.identity);

                Destroy(hitObject);
            }
            else
            {
                Debug.Log("Hit non-diggable object: " + hitObject.name);
            }
        }
        else
        {
            Debug.Log("❌ Raycast did not hit anything.");
        }
    }

}
