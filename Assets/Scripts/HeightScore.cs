using TMPro;
using UnityEngine;

public class HeightScore : MonoBehaviour
{
    // the frog whose height is tracked
    [SerializeField] private Transform frog;

    // UI text displaying the score
    [SerializeField] private TMP_Text heightText;

    [SerializeField] private TMP_Text bestScoreText;

    private float highestY;
    private float startingY;
    private int flyPoints;
    private int bestScore;

    private const string BestScoreKey = "FroggyHeightsBestScore";

    private void Start()
    {
        startingY = frog.position.y;
        highestY = startingY;

        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);

        UpdateScoreDisplay();
    }

    private void Update()
    {
        // updates the height score when the frog reaches a new height
        if (frog.position.y > highestY)
        {
            highestY = frog.position.y;
            UpdateScoreDisplay();
        }
    }

    // adds points when the frog collects a fly
    public void AddPoints(int points)
    {
        flyPoints += points;
        UpdateScoreDisplay();
    }

    // updates and saves the current best score
    private void UpdateScoreDisplay()
    {
        int heightScore = Mathf.FloorToInt(
            (highestY - startingY) * 10f
        );

        int totalScore = heightScore + flyPoints;

        if (totalScore > bestScore)
        {
            bestScore = totalScore;

            PlayerPrefs.SetInt(
                BestScoreKey,
                bestScore
            );

            PlayerPrefs.Save();
        }

        heightText.text =
            "Height: " + heightScore +
            "\nFlies: " + flyPoints +
            "\nScore: " + totalScore;

        bestScoreText.text = "High Score: " + bestScore;
    }
}