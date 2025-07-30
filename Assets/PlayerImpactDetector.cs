using UnityEngine;

public class PlayerImpactDetector : MonoBehaviour
{
    public float impactVelocityThreshold = 10f;

    void OnTriggerEnter(Collider other)
    {
        Rigidbody debrisRb = other.attachedRigidbody;

        if (debrisRb != null && debrisRb.linearVelocity.magnitude > impactVelocityThreshold)
        {
            Debug.Log("☠️ Triggered by high-speed debris: " + other.name);

            GameOverManager gameOver = FindFirstObjectByType<GameOverManager>();
            if (gameOver != null)
            {
                gameOver.TriggerLoss("You were hit by debris!");
            }
        }
    }
}
