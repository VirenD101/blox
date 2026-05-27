using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Level Constraints")]
    public int levelPar = 15; // Set par ... use array for more levels later
    private int moveCount = 0;

    [Header("UI Components")]
    public TextMeshProUGUI scoreText;

    void Awake()
    {
        // Setup Singleton pattern
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        moveCount = 0;
        UpdateScoreUI();
    }

    // Call this whenever the block successfully executes a flip
    public void RecordMove()
    {
        moveCount++;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"MOVES: <color=#00ffcc>{moveCount}</color> PAR: <b>{levelPar}</b>";
        }
    }

    // This returns performance data when the player reaches the portal
    public int GetFinalScore()
    {
        return moveCount;
    }
}