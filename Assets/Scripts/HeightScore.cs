using TMPro;
using UnityEngine;

public class HeightScore : MonoBehaviour
{
    // frog whose highest height is tracked
    [SerializeField] private Transform frog;

    // text displaying the current score
    [SerializeField] private TMP_Text heightText;

    // text displaying the saved high score
    [SerializeField] private TMP_Text bestScoreText;

    private float highestY;
    private float startingY;
    private int flyPoints;
    private int bestScore;

    // key used to save the high score
    private const string BestScoreKey =
        "FroggyHeightsBestScore";

    // records the frog's starting height and loads the high score
    private void Start()
    {
        startingY = frog.position.y;
        highestY = startingY;

        bestScore =
            PlayerPrefs.GetInt(
                BestScoreKey,
                0
            );

        UpdateScoreDisplay();
    }

    // updates the height score when the frog climbs higher
    private void Update()
    {
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

    // updates the score display and saves the high score
    private void UpdateScoreDisplay()
    {
        int heightScore =
            Mathf.FloorToInt(
                (highestY - startingY) * 10f
            );

        int totalScore =
            heightScore + flyPoints;

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

        bestScoreText.text =
            "High Score: " + bestScore;
    }
}