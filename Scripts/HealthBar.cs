using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider; // Assign a Slider UI in Inspector
    public float displayTime = 3f; // How long it shows after hit
    private float timer;
    private Transform target; // Enemy to follow
    private Canvas canvas;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        if (healthSlider == null)
        {
            Debug.LogError("HealthSlider not assigned in HealthBar!");
        }
        gameObject.SetActive(false); // Hidden by default
    }

    void Update()
    {
        if (target != null)
        {
            // Position above the enemy in screen space
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + Vector3.up * 2f);
            transform.position = screenPos;
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                gameObject.SetActive(false); // Hide after timer expires
            }
        }
    }

    public void ShowHealthBar(float currentHealth, float maxHealth, Transform enemyTarget)
    {
        target = enemyTarget;
        healthSlider.value = currentHealth / maxHealth;
        gameObject.SetActive(true);
        timer = displayTime;
    }
}