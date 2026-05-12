using UnityEngine;

public class UFOHealth : MonoBehaviour
{
    public int maxHits = 5;
    private int hitsRemaining;
    private GameManager gameManager;

    void Start()
    {
        hitsRemaining = maxHits;
        gameManager = Object.FindFirstObjectByType<GameManager>();
    }

    public void TakeDamage()
    {
        if (gameManager != null && !gameManager.gameActive) return;
        hitsRemaining--;
        if (hitsRemaining <= 0)
        {
            hitsRemaining = 0;
            gameManager?.EndGame(false);
        }
    }

    public int HitsRemaining => hitsRemaining;
}
