using TMPro;
using UnityEngine;

public class HeightScore : MonoBehaviour
{
    // the frog whose highest height is being tracked
    [SerializeField] private Transform frog;

    // UI text that displays the score
    [SerializeField] private TMP_Text heightText;

    // stores the frog's highest recorded position
    private float highestY;

    // stores the frog's starting height
    private float startingY;

    // stores points collected from flies
    private int flyPoints;

    private void Start()
    {
        startingY = frog.position.y;
        highestY = startingY;
        UpdateScoreDisplay();
    }

    private void Update()
    {
        // only increase the height score when the frog reaches a new height
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

    // updates the UI with height and fly points
    private void UpdateScoreDisplay()
    {
        int heightScore = Mathf.FloorToInt(
            (highestY - startingY) * 10f
        );

        heightText.text =
            "Height: " + heightScore +
            "\nFlies: " + flyPoints;
    }
}