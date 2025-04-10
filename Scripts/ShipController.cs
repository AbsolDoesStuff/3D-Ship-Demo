using UnityEngine;
using UnityEngine.UI;

public class ShipController : MonoBehaviour
{
    // Movement variables
    public float moveSpeed = 5f;
    public float strafeSpeed = 3f;
    public float rollSpeed = 180f;
    public float yawSpeed = 60f;
    public float pitchSpeed = 60f;
    private Rigidbody rb;

    // Health variables
    public float maxHealth = 100f;
    private float currentHealth;

    // Projectile variables
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float nextFireTime;

    // Camera
    public Camera mainCamera;

    // Visual cube for barrel roll
    public Transform cubeVisual;
    private float cubeRollAngle = 0f;
    private float cubeRollInput = 0f;

    // Physics damping
    public float angularDrag = 5f;

    // Player health bar UI
    public Slider playerHealthBar;
    public Text playerHealthText;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody not found on ShipController! Adding one...");
            rb = gameObject.AddComponent<Rigidbody>();
        }

        currentHealth = maxHealth;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.angularDamping = angularDrag;

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            Debug.LogWarning("MainCamera not assigned; using Camera.main.");
        }

        if (cubeVisual == null)
        {
            Debug.LogError("CubeVisual not assigned in ShipController!");
        }

        if (mainCamera.transform.parent != transform)
        {
            mainCamera.transform.SetParent(transform, false);
            mainCamera.transform.localPosition = new Vector3(0f, 2f, -5f);
        }
        mainCamera.transform.localRotation = Quaternion.identity;

        transform.position = new Vector3(0f, 10f, 0f);
        Debug.Log($"Ship initialized at {transform.position}. Health: {currentHealth}");

        // Initialize player health bar
        if (playerHealthBar != null)
        {
            playerHealthBar.gameObject.SetActive(true); // Ensure it's visible
            playerHealthBar.maxValue = maxHealth;
            playerHealthBar.value = currentHealth;
            Debug.Log("Player health bar activated.");
            if (playerHealthText != null)
            {
                playerHealthText.text = "HP";
                playerHealthText.gameObject.SetActive(true); // Ensure text is visible
                Debug.Log("Player health text set to 'HP' and activated.");
            }
            else
            {
                Debug.LogWarning("PlayerHealthText not assigned in ShipController!");
            }
        }
        else
        {
            Debug.LogError("PlayerHealthBar not assigned in ShipController!");
        }
    }

    void Update()
    {
        HandleMovement();
        HandleCubeRoll();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextFireTime)
        {
            FireProjectile();
            nextFireTime = Time.time + fireRate;
        }

        Debug.Log($"Angular Velocity: {rb.angularVelocity.magnitude}");
    }

    void HandleMovement()
    {
        Vector3 movement = Vector3.zero;
        float yawInput = 0f;
        float pitchInput = 0f;
        float rollInput = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            movement += transform.forward * moveSpeed;
            Debug.Log("W pressed, moving forward");
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement -= transform.forward * moveSpeed;
            Debug.Log("S pressed, moving backward");
        }
        if (Input.GetKey(KeyCode.Q))
        {
            movement -= transform.right * strafeSpeed;
            Debug.Log("Q pressed, strafing left");
        }
        if (Input.GetKey(KeyCode.E))
        {
            movement += transform.right * strafeSpeed;
            Debug.Log("E pressed, strafing right");
        }

        if (Input.GetKey(KeyCode.A)) yawInput = -1f;
        if (Input.GetKey(KeyCode.D)) yawInput = 1f;
        if (Input.GetKey(KeyCode.Z)) pitchInput = 1f;
        if (Input.GetKey(KeyCode.C)) pitchInput = -1f;
        if (Input.GetKey(KeyCode.Q)) rollInput = 1f;
        if (Input.GetKey(KeyCode.E)) rollInput = -1f;

        rb.MovePosition(rb.position + movement * Time.deltaTime);

        if (yawInput != 0f || pitchInput != 0f || rollInput != 0f)
        {
            Quaternion rotation = Quaternion.Euler(pitchInput * pitchSpeed * Time.deltaTime, 
                                                  yawInput * yawSpeed * Time.deltaTime, 
                                                  rollInput * rollSpeed * Time.deltaTime);
            rb.MoveRotation(rb.rotation * rotation);
        }

        if (Input.GetKey(KeyCode.Q)) cubeRollInput = -1f;
        else if (Input.GetKey(KeyCode.E)) cubeRollInput = 1f;
        else cubeRollInput = 0f;
    }

    void HandleCubeRoll()
    {
        if (cubeVisual != null)
        {
            if (cubeRollInput != 0f)
            {
                cubeRollAngle += cubeRollInput * rollSpeed * Time.deltaTime;
            }
            else if (!Mathf.Approximately(cubeRollAngle % 360f, 0f))
            {
                float direction = cubeRollAngle > 0f ? -1f : 1f;
                cubeRollAngle += direction * rollSpeed * Time.deltaTime;
                if (Mathf.Abs(cubeRollAngle % 360f) < rollSpeed * Time.deltaTime)
                {
                    cubeRollAngle = Mathf.Round(cubeRollAngle / 360f) * 360f;
                }
            }
            cubeVisual.localRotation = Quaternion.Euler(cubeRollAngle, 0f, 0f);
        }
    }

    void FireProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            SoundManager.Instance.PlayProjectileSound(firePoint.position);
            Debug.Log("Projectile fired from: " + firePoint.position);
        }
        else
        {
            Debug.LogWarning("ProjectilePrefab or FirePoint not assigned!");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Ship took {damage} damage. Current health: {currentHealth}");
        SoundManager.Instance.PlayHitSound(transform.position);

        if (playerHealthBar != null)
        {
            playerHealthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Ship destroyed!");
            GameManager.Instance.OnObjectDestroyed(gameObject, transform.position);
            Destroy(gameObject);
        }
    }

    public void RestoreHealth(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"Health restored by {amount}. Current health: {currentHealth}");

        if (playerHealthBar != null)
        {
            playerHealthBar.value = currentHealth;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Ship collided with: {collision.gameObject.name}, Tag: {collision.gameObject.tag}, Position: {transform.position}, Angular Velocity Before: {rb.angularVelocity.magnitude}");

        rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, 0.5f);

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Debris"))
        {
            EnemyAndDebris enemyDebris = collision.gameObject.GetComponent<EnemyAndDebris>();
            if (enemyDebris != null)
            {
                TakeDamage(enemyDebris.damageOutput);
            }
            else
            {
                Debug.LogWarning("No EnemyAndDebris component found on collided object!");
            }
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Ship hit the ground!");
            TakeDamage(maxHealth);
        }

        Debug.Log($"Angular Velocity After: {rb.angularVelocity.magnitude}");
    }
}