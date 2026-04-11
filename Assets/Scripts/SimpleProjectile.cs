using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private bool initialized = false;

    public void Initialize(Vector3 direction, float speed)
    {
        this.direction = direction.normalized;
        this.speed = speed;
        initialized = true;
    }

    void Update()
    {
        if (!initialized)
        {
            return;
        }

        transform.position += direction * speed * Time.deltaTime;
    }
}
