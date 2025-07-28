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
        hasExploded = true;

        Vector3 impactPoint = transform.position;

        // Get nearby objects
        Collider[] colliders = Physics.OverlapSphere(impactPoint, explosionRadius);
        foreach (Collider nearby in colliders)
        {
            Destructable dw = nearby.GetComponent<Destructable>();
            if (dw != null)
            {
                dw.Break(impactPoint, explosionForce, explosionRadius);
            }

            if (dw != null)
            {
                dw.isKinematic = false;
                dw.useGravity = true;
                dw.AddExplosionForce(explosionForce, impactPoint, explosionRadius, 3f, ForceMode.Impulse);
            }
        }

        if (explosionEffect)
            Instantiate(explosionEffect, impactPoint, Quaternion.identity);

        Destroy(gameObject);
    }

}
