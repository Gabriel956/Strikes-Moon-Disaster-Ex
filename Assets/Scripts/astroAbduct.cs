using UnityEngine;

public class astroAbduct : MonoBehaviour
{
    public bool isBeingAbducted = false;
    public float riseSpeed = 3f;
    public float absorptionRadius = 0.3f;

    private Transform ufo;
    private GameManager gameManager;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void StartAbduction(Transform ufoTransform, GameManager gm)
    {
        if (isBeingAbducted) return;
        isBeingAbducted = true;
        ufo = ufoTransform;
        gameManager = gm;

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        foreach (Collider col in GetComponentsInChildren<Collider>())
            col.enabled = false;
    }

    void Update()
    {
        if (!isBeingAbducted || ufo == null) return;

        transform.position = Vector3.MoveTowards(transform.position, ufo.position, riseSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, ufo.position) < absorptionRadius)
        {
            gameManager.AddAbduction();
            Destroy(gameObject);
        }
    }
}
