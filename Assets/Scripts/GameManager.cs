using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int abductedCount = 0;
    public int targetCount = 5;

    public float timeRemaining = 20f;
    public bool gameActive = true;

    private UFOHealth ufoHealth;

    void Start()
    {
        ufoHealth = Object.FindFirstObjectByType<UFOHealth>();
    }

    void Update()
    {
        if (!gameActive) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            EndGame(false);
        }
    }

    public void AddAbduction()
    {
        if (!gameActive) return;

        abductedCount++;

        Debug.Log("Abducted: " + abductedCount);

        if (abductedCount >= targetCount)
            EndGame(true);
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 48;
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(10, 10, 600, 60), "Abducted: " + abductedCount, style);
        GUI.Label(new Rect(10, 75, 600, 60), "Time: " + Mathf.Ceil(timeRemaining), style);
        int hits = (ufoHealth != null) ? ufoHealth.HitsRemaining : 3;
        GUI.Label(new Rect(10, 140, 600, 60), "Health: " + hits, style);
    }

    public void EndGame(bool won)
    {
        gameActive = false;
        Debug.Log(won ? "NEXT LEVEL!" : "YOU LOST...");
        Time.timeScale = 0f;
    }
}
