using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AsteroidFall : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 10f;
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

        Collider[] colliders = Physics.OverlapSphere(impactPoint, explosionRadius);
        foreach (Collider nearby in colliders)
        {
            Destructable dw = nearby.GetComponent<Destructable>();
            if (dw != null)
            {
                dw.Break(impactPoint, explosionForce, explosionRadius);
            }
        }

        if (explosionEffect)
            Instantiate(explosionEffect, impactPoint, Quaternion.identity);

        //Destroy(gameObject);
    }


}
