using UnityEngine;

public class CannonProjectileHit : MonoBehaviour
{
    public float lifetime = 5f;

    void Start() => Destroy(gameObject, lifetime);

    void OnCollisionEnter(Collision collision)
    {
        UFOHealth ufoHealth = collision.gameObject.GetComponentInParent<UFOHealth>();
        if (ufoHealth != null)
            ufoHealth.TakeDamage();
        Destroy(gameObject);
    }
}
