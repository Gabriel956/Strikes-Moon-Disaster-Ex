using UnityEngine;

public class AbductionZone : MonoBehaviour
{
    public GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        Transform astronautRoot = other.transform.root;

        if (!astronautRoot.CompareTag("astro")) return;

        astroAbduct astro = astronautRoot.GetComponent<astroAbduct>();
        if (astro != null && !astro.isBeingAbducted)
            astro.StartAbduction(transform.parent, gameManager);
    }
}
