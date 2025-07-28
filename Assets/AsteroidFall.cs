using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AsteroidFall : MonoBehaviour
{
    public float explosionRadius = 50f;
    public float explosionForce = 1500f;
    public GameObject explosionEffect;

    private bool hasExploded = false;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true; // Keeps it frozen until triggered
    }

    public void Release()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;

        // Optional: only respond to Ground
       // if (!collision.collider.CompareTag("Destructable")) return;

        hasExploded = true;

        Vector3 impactPoint = transform.position;

        Collider[] colliders = Physics.OverlapSphere(impactPoint, explosionRadius);
        foreach (Collider nearby in colliders)
        {
            Rigidbody nearbyRb = nearby.attachedRigidbody;
            if (nearbyRb != null)
            {
                nearbyRb.isKinematic = false;
                nearbyRb.useGravity = true;
                nearbyRb.AddExplosionForce(explosionForce, impactPoint, explosionRadius, 3f, ForceMode.Impulse);
            }
        }

        if (explosionEffect)
            Instantiate(explosionEffect, impactPoint, Quaternion.identity);

        Debug.Log("☄️ Asteroid impact triggered by collision.");
        Destroy(gameObject);
    }
}
