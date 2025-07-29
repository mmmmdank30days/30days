using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Destructable : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasBroken = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;         // Initially static
        rb.useGravity = false;
        //gameObject.tag = "Destructable"; // Required for asteroid to detect
    }

    // Call this when the asteroid explodes nearby
    public void Break(Vector3 explosionOrigin, float explosionForce, float explosionRadius)
    {
        if (hasBroken) return;

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddExplosionForce(explosionForce, explosionOrigin, explosionRadius, 3f, ForceMode.Impulse);
        hasBroken = true;
    }
}
