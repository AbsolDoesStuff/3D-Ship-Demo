using UnityEngine;

public class ShipController : MonoBehaviour
{
    private Rigidbody rb;
    public float thrustSpeed = 15f;
    public float rotationSpeed = 100f;
    public float strafeSpeed = 8f;
    public GameObject projectilePrefab; // Drag prefab here
    public Transform firePoint;         // Drag FirePoint child here

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
            rb.AddForce(transform.forward * thrustSpeed, ForceMode.Acceleration);
        if (Input.GetKey(KeyCode.S))
            rb.AddForce(-transform.forward * thrustSpeed, ForceMode.Acceleration);

        float yaw = Input.GetKey(KeyCode.A) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f;
        transform.Rotate(0, yaw * rotationSpeed * Time.deltaTime, 0);

        if (Input.GetKey(KeyCode.Q))
        {
            rb.AddForce(-transform.right * strafeSpeed, ForceMode.Acceleration);
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            rb.AddForce(transform.right * strafeSpeed, ForceMode.Acceleration);
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }

        float pitch = Input.GetKey(KeyCode.Z) ? 1f : Input.GetKey(KeyCode.C) ? -1f : 0f;
        transform.Rotate(pitch * rotationSpeed * Time.deltaTime, 0, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }

    void OnCollisionEnter(Collision collision)
    {
        Health health = GetComponent<Health>();
        if (health != null) health.TakeDamage(20f);
    }
}