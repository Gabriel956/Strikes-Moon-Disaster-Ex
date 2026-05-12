using UnityEngine;

public class AbductionZone : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject beamVisual;

    void Start()
    {
        if (beamVisual != null)
            beamVisual.SetActive(gameManager == null || !gameManager.highScoreMode);
    }

    private void OnTriggerStay(Collider other)
    {
        if (gameManager != null && gameManager.highScoreMode) return;

        Transform astronautRoot = other.transform.root;

        if (!astronautRoot.CompareTag("astro")) return;

        astroAbduct astro = astronautRoot.GetComponent<astroAbduct>();
        if (astro != null && !astro.isBeingAbducted)
            astro.StartAbduction(transform.parent, gameManager);
    }
}
