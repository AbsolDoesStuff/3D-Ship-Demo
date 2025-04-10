using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject enemyPrefab;
    public GameObject debrisPrefab;
    public int maxEnemies = 5;
    public int maxDebris = 10;
    public ParticleSystem explosionEffect;
    public GameObject menuScreen;

    // Boss UI
    public Slider bossHealthBar;
    public Text bossHealthText;
    private BossController activeBoss;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        Time.timeScale = 0f;
    }

    void Start()
    {
        // Ensure menu screen is shown
        if (menuScreen != null)
        {
            menuScreen.SetActive(true);
            Debug.Log("Menu screen activated at game start.");
        }
        else
        {
            Debug.LogError("MenuScreen not assigned in GameManager!");
        }

        // Boss health bar is hidden until game starts and a boss is found
        if (bossHealthBar != null)
        {
            bossHealthBar.gameObject.SetActive(false);
            Debug.Log("Boss health bar initialized (hidden).");
        }
        else
        {
            Debug.LogError("BossHealthBar not assigned in GameManager!");
        }

        // Spawn enemies and debris
        SpawnInitialObjects();
    }

    void Update()
    {
        if (activeBoss != null && bossHealthBar != null)
        {
            bossHealthBar.value = activeBoss.GetCurrentHealth() / activeBoss.maxHealth;
            if (bossHealthText != null)
            {
                bossHealthText.text = "Boss HP";
            }
        }
    }

    void SpawnInitialObjects()
    {
        // Validate prefabs before spawning
        if (enemyPrefab == null)
        {
            Debug.LogError("EnemyPrefab not assigned in GameManager! Cannot spawn enemies.");
            return;
        }
        if (debrisPrefab == null)
        {
            Debug.LogError("DebrisPrefab not assigned in GameManager! Cannot spawn debris.");
            return;
        }

        Debug.Log($"Spawning {maxEnemies} enemies...");
        for (int i = 0; i < maxEnemies; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-30f, 30f), Random.Range(5f, 10f), Random.Range(-30f, 30f));
            Debug.Log($"Spawning enemy {i + 1} at {spawnPos}");
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            enemy.tag = "Enemy";
        }

        Debug.Log($"Spawning {maxDebris} debris...");
        for (int i = 0; i < maxDebris; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-30f, 30f), Random.Range(5f, 10f), Random.Range(-30f, 30f));
            Debug.Log($"Spawning debris {i + 1} at {spawnPos}");
            GameObject debris = Instantiate(debrisPrefab, spawnPos, Quaternion.identity);
            debris.tag = "Debris";
        }
    }

    public void OnObjectDestroyed(GameObject obj, Vector3 position, bool playSound = true)
    {
        if (playSound)
        {
            SoundManager.Instance.PlayExplosionSound(position);
        }
        if (explosionEffect != null)
        {
            ParticleSystem effect = Instantiate(explosionEffect, position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
        }

        if (obj.CompareTag("Enemy"))
        {
            Vector3 spawnPos = new Vector3(Random.Range(-30f, 30f), Random.Range(5f, 10f), Random.Range(-30f, 30f));
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity).tag = "Enemy";
        }
        else if (obj.CompareTag("Debris"))
        {
            Vector3 spawnPos = new Vector3(Random.Range(-30f, 30f), Random.Range(5f, 10f), Random.Range(-30f, 30f));
            Instantiate(debrisPrefab, spawnPos, Quaternion.identity).tag = "Debris";
        }
    }

    public void StartGame()
    {
        if (menuScreen != null)
        {
            menuScreen.SetActive(false);
            Debug.Log("Start button pressed. Menu hidden, game unpaused.");
        }
        Time.timeScale = 1f;

        // Find and activate boss UI if present
        activeBoss = FindObjectOfType<BossController>();
        if (activeBoss != null && bossHealthBar != null)
        {
            bossHealthBar.gameObject.SetActive(true);
            bossHealthBar.GetComponent<Image>().color = Color.red;
            Debug.Log("Boss found, health bar activated.");
        }
        else
        {
            Debug.LogWarning("No BossController found in scene or BossHealthBar not assigned.");
        }
    }

    public void RegisterBoss(BossController boss)
    {
        activeBoss = boss;
        if (bossHealthBar != null)
        {
            bossHealthBar.gameObject.SetActive(true);
            bossHealthBar.GetComponent<Image>().color = Color.red;
            Debug.Log("Boss registered, health bar activated.");
        }
    }
}