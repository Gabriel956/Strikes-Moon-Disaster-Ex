using UnityEngine;

public class CannonShoot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate = 3f;
    public float projectileSpeed = 8f;

    private Transform ufo;
    private GameManager gameManager;

    void Start()
    {
        gameManager = Object.FindFirstObjectByType<GameManager>();

        UFOHealth health = Object.FindFirstObjectByType<UFOHealth>();
        if (health != null)
            ufo = health.transform;
        else
            Debug.LogWarning("CannonShoot on " + gameObject.name + ": UFOHealth not found in scene.");

        if (projectilePrefab == null)
            Debug.LogWarning("CannonShoot on " + gameObject.name + ": projectilePrefab is not assigned.");

        InvokeRepeating(nameof(Fire), fireRate, fireRate);
    }

    void Fire()
    {
        if (ufo == null || projectilePrefab == null) return;
        if (gameManager != null && !gameManager.gameActive) return;

        Vector3 direction = (ufo.position - transform.position).normalized;
        Vector3 spawnPos = transform.position + direction * 1.5f;
        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Collider[] projColliders = proj.GetComponentsInChildren<Collider>();
        Collider[] baseColliders = transform.root.GetComponentsInChildren<Collider>();
        foreach (Collider pc in projColliders)
            foreach (Collider bc in baseColliders)
                Physics.IgnoreCollision(pc, bc);

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = direction * projectileSpeed;
        else
        {
            SimpleProjectile sp = proj.GetComponent<SimpleProjectile>();
            if (sp != null) sp.Initialize(direction, projectileSpeed);
            else
                Debug.LogWarning("CannonShoot: projectilePrefab has no Rigidbody or SimpleProjectile component.");
        }
    }
}
