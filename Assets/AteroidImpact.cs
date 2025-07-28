using UnityEngine;

public class AsteroidImpact : MonoBehaviour
{
    [Header("Impact Settings")]
    public float impactRadius = 50f;
    public float impactForce = 1500f;
    public Vector3 impactPoint = Vector3.zero;

    [Header("Effects")]
    public GameObject explosionEffect;

    /// <summary>
    /// Triggers the asteroid impact by applying force to nearby rigidbodies.
    /// </summary>
    public void TriggerImpact()
    {
        // Get all colliders in the impact radius
        Collider[] colliders = Physics.OverlapSphere(impactPoint, impactRadius);
        foreach (Collider nearby in colliders)
        {
            Rigidbody rb = nearby.attachedRigidbody;
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;

                rb.AddExplosionForce(
                    impactForce,
                    impactPoint,
                    impactRadius,
                    3f, // upwards modifier
                    ForceMode.Impulse
                );
            }
        }

        // Optional: spawn visual effect
        if (explosionEffect)
        {
            Instantiate(explosionEffect, impactPoint, Quaternion.identity);
        }

        Debug.Log("💥 Asteroid impact triggered at " + impactPoint);
    }
}
