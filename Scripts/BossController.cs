using UnityEngine;

public class BossController : MonoBehaviour
{
    public float maxHealth = 500f;
    public float damageOutput = 20f;
    private float currentHealth;

    public HealthBar healthBarPrefab;

    void Start()
    {
        currentHealth = maxHealth;
        GameManager.Instance.RegisterBoss(this);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Boss took {damage} damage. Current health: {currentHealth}");
        SoundManager.Instance.PlayHitSound(transform.position);

        if (healthBarPrefab != null)
        {
            HealthBar bar = Instantiate(healthBarPrefab, GameManager.Instance.transform);
            bar.ShowHealthBar(currentHealth, maxHealth, transform);
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Boss destroyed!");
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

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}