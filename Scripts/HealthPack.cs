using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public float healthRestore = 50f; // Amount to restore
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = false; // Non-lethal, floats in space
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ShipController player = collision.gameObject.GetComponent<ShipController>();
            if (player != null)
            {
                player.RestoreHealth(healthRestore);
                Debug.Log($"Health pack picked up, restoring {healthRestore} health.");
                Destroy(gameObject); // Remove health pack after pickup
            }
        }
    }
}