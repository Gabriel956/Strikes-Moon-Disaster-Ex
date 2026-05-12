using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int abductedCount = 0;
    public int targetCount = 5;

    public float timeRemaining = 20f;
    public bool gameActive = true;

    public string nextSceneName = "";
    public bool highScoreMode = false;
    public int score = 0;

    private UFOHealth ufoHealth;

    void Start()
    {
        Time.timeScale = 1f;
        ufoHealth = Object.FindFirstObjectByType<UFOHealth>();
    }

    void Update()
    {
        if (!gameActive) return;
        if (highScoreMode) return;

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
        if (highScoreMode) { score++; return; }

        abductedCount++;

        Debug.Log("Abducted: " + abductedCount);

        if (abductedCount >= targetCount)
            EndGame(true);
    }

    public void AddScore()
    {
        if (!gameActive) return;
        score++;
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 48;
        style.normal.textColor = Color.white;

        if (highScoreMode)
        {
            GUI.Label(new Rect(10, 10, 600, 60), "Score: " + score, style);
        }
        else
        {
            GUI.Label(new Rect(10, 10, 600, 60), "Abducted: " + abductedCount, style);
            GUI.Label(new Rect(10, 75, 600, 60), "Time: " + Mathf.Ceil(timeRemaining), style);
        }

        int hits = (ufoHealth != null) ? ufoHealth.HitsRemaining : 3;
        GUI.Label(new Rect(10, 140, 600, 60), "Health: " + hits, style);
    }

    public void EndGame(bool won)
    {
        gameActive = false;
        Time.timeScale = 0f;
        if (won)
        {
            Debug.Log("NEXT LEVEL!");
            if (!string.IsNullOrEmpty(nextSceneName))
                SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.Log("YOU LOST...");
        }
    }
}
