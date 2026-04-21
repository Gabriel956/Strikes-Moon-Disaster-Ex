using UnityEngine;

public class AbductionZone : MonoBehaviour
{
    public float pullSpeed = 2f;
    public GameManager gameManager;

    private void OnTriggerStay(Collider other)
{
    Transform enemyTransform = other.transform.root;

    if (enemyTransform.CompareTag("astro"))
    {
        astroAbduct astro = enemyTransform.GetComponent<astroAbduct>();

        if (astro != null && !astro.isBeingAbducted)
        {
            astro.isBeingAbducted = true; 

            Rigidbody rb = enemyTransform.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity = new Vector3(0, pullSpeed, 0);
            }

            float distance = Vector3.Distance(
                enemyTransform.position,
                transform.parent.position
            );

            if (distance < 4f)
            {
                gameManager.AddAbduction();
                Destroy(enemyTransform.gameObject);
            }
        }
    }
}
}
