using UnityEngine;

public class MeteorLogic : MonoBehaviour
{
    public float gravityMultiplier = 2f;
    public float maxFallSpeed = 50f;
    public float initialSpeed = 10f;
    private Rigidbody rb;
    private bool hasHit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.down * initialSpeed;
    }

    void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
        if (rb.linearVelocity.y < -maxFallSpeed)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -maxFallSpeed, rb.linearVelocity.z);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        hasHit = true;

        UFOHealth ufoHealth = collision.gameObject.GetComponentInParent<UFOHealth>();
        if (ufoHealth != null)
            ufoHealth.TakeDamage();

        Destroy(gameObject);
    }
}
