using UnityEngine;

public class ExhaustController : MonoBehaviour
{
    private ParticleSystem ps;
    private Rigidbody rb;

    void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var emission = ps.emission;
        emission.rateOverTime = 50f + (rb.linearVelocity.magnitude * 5f); // Scales with speed
    }
}