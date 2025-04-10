using UnityEngine;

public class MinionController : MonoBehaviour
{
    public float maxHealth = 100f;
    public float damageOutput = 10f;
    public float moveSpeed = 2f;
    private float currentHealth;
    private Transform playerTarget;
    private Rigidbody rb;

    public HealthBar healthBarPrefab;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        playerTarget = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTarget == null)
        {
            Debug.LogWarning("MinionController: No player found with tag 'Player'!");
        }
    }

    void Update()
    {
        if (playerTarget != null)
        {
            Vector3 direction = (playerTarget.position - transform.position).normalized;
            Vector3 movement = direction * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
            Debug.Log($"Minion moving toward player at {playerTarget.position}. Current position: {transform.position}");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Minion took {damage} damage. Current health: {currentHealth}");
        SoundManager.Instance.PlayHitSound(transform.position);

        if (healthBarPrefab != null)
        {
            HealthBar bar = Instantiate(healthBarPrefab, GameManager.Instance.transform);
            bar.ShowHealthBar(currentHealth, maxHealth, transform);
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Minion destroyed!");
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