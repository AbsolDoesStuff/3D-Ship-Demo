using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    public float damage = 20f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Debris"))
        {
            // Check for EnemyAndDebris
            EnemyAndDebris enemyDebris = collision.gameObject.GetComponent<EnemyAndDebris>();
            if (enemyDebris != null)
            {
                enemyDebris.TakeDamage(damage);
                Debug.Log($"Projectile hit {collision.gameObject.name} (EnemyAndDebris) and dealt {damage} damage.");
            }

            // Check for MinionController
            MinionController minion = collision.gameObject.GetComponent<MinionController>();
            if (minion != null)
            {
                minion.TakeDamage(damage);
                Debug.Log($"Projectile hit {collision.gameObject.name} (Minion) and dealt {damage} damage.");
            }

            // Check for BossController
            BossController boss = collision.gameObject.GetComponent<BossController>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
                Debug.Log($"Projectile hit {collision.gameObject.name} (Boss) and dealt {damage} damage.");
            }

            // Destroy projectile with particle effect only
            GameManager.Instance.OnObjectDestroyed(gameObject, transform.position, false);
            Destroy(gameObject);
        }
    }
}