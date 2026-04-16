using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int abductedCount = 0;
    public int targetCount = 5;

    public float timeRemaining = 20f;
    public bool gameActive = true;

    void Update()
    {
        if (!gameActive) return;

        // Countdown timer
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            EndGame(false); // lose
        }
    }

    public void AddAbduction()
    {
        if (!gameActive) return;

        abductedCount++;

        Debug.Log("Abducted: " + abductedCount);

        if (abductedCount >= targetCount)
        {
            EndGame(true); // win
        }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 400;
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(10, 10, 200, 20), "Abducted: " + abductedCount);
        GUI.Label(new Rect(10, 30, 200, 20), "Time: " + Mathf.Ceil(timeRemaining));
    }

    void EndGame(bool won)
    {
        gameActive = false;

        if (won)
        {
            Debug.Log("NEXT LEVEL!");
        }
        else
        {
            Debug.Log("YOU LOST...");
        }

        Time.timeScale = 0f;
    }
}