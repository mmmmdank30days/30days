using UnityEngine;

public class AsteroidImpact : MonoBehaviour
{
    public float impactRadius = 50f;
    public float impactForce = 1500f;
    public Vector3 impactPoint = Vector3.zero;
    public GameObject explosionEffect; // Optional particle effect

    public void TriggerImpact()
    {
        Collider[] colliders = Physics.OverlapSphere(impactPoint, impactRadius);
        foreach (Collider nearby in colliders)
        {
            Rigidbody rb = nearby.attachedRigidbody;
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddExplosionForce(impactForce, impactPoint, impactRadius, 3f, ForceMode.Impulse);
            }
        }

        if (explosionEffect)
            Instantiate(explosionEffect, impactPoint, Quaternion.identity);

        Debug.Log("Asteroid impact triggered.");
    }
}
