using UnityEngine;

public class ProjectileHit : MonoBehaviour
{
    [SerializeField] private string targetTag = "astro";
    [SerializeField] private bool destroyProjectileOnHit = true;

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
        if (hitTransform == null)
        {
            return;
        }

        GameObject target = null;
        if (hitTransform.CompareTag(targetTag))
        {
            target = hitTransform.gameObject;
        }
        else if (hitTransform.root != null && hitTransform.root.CompareTag(targetTag))
        {
            target = hitTransform.root.gameObject;
        }

        if (target == null)
        {
            return;
        }

        Destroy(target);
        if (destroyProjectileOnHit)
        {
            Destroy(gameObject);
        }
    }
}
