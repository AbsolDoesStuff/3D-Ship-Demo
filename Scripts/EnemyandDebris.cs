using UnityEngine;

public class EnemyAndDebris : MonoBehaviour
{
    public float maxHealth = 50f;
    public float damageOutput = 10f;
    private float currentHealth;

    public HealthBar healthBarPrefab;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");
        SoundManager.Instance.PlayHitSound(transform.position);

        if (healthBarPrefab != null)
        {
            HealthBar bar = Instantiate(healthBarPrefab, GameManager.Instance.transform);
            bar.ShowHealthBar(currentHealth, maxHealth, transform);
        }

        if (currentHealth <= 0)
        {
            Debug.Log($"{gameObject.name} destroyed!");
            GameManager.Instance.OnObjectDestroyed(gameObject, transform.position);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ShipController player = collision.gameObject.GetComponent<ShipController>();
            if (player != null)
            {
                player.TakeDamage(damageOutput);
            }
        }
    }
}