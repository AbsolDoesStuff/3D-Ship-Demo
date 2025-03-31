using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10f;
    public float lifetime = 5f; // Auto-destroy after 5s

    void Start()
    {
        Destroy(gameObject, lifetime); // Clean up
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // Move forward
    }

    void OnCollisionEnter(Collision collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null) health.TakeDamage(damage);
        Destroy(gameObject); // Destroy on hit
    }
}