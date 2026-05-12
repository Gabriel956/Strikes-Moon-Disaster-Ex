using UnityEngine;

public class ProjectileHit : MonoBehaviour
{
    [SerializeField] private bool destroyProjectileOnHit = true;

    private GameManager gameManager;
    private bool hasHit = false;

    void Start()
    {
        gameManager = Object.FindFirstObjectByType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        TryHit(other.transform);
    }

    void OnCollisionEnter(Collision collision)
    {
        TryHit(collision.transform);
    }

    private void TryHit(Transform hitTransform)
    {
        if (hasHit) return;
        if (hitTransform == null) return;

        hasHit = true;

        astroAbduct astro = hitTransform.GetComponent<astroAbduct>()
                         ?? hitTransform.GetComponentInParent<astroAbduct>();

        if (astro != null)
        {
            gameManager?.AddScore();
            Destroy(astro.gameObject);
        }

        if (destroyProjectileOnHit)
            Destroy(gameObject);
    }
}
