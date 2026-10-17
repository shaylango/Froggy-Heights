using TMPro;
using UnityEngine;

public class HeightScore : MonoBehaviour
{
    // frog whose highest height is being tracked
    [SerializeField] private Transform frog;

    // UI text that displays the score
    [SerializeField] private TMP_Text heightText;

    // stores the frog's highest recorded position
    private float highestY;

    // stores the frog's starting height
    private float startingY;

    private void Start()
    {
        startingY = frog.position.y;
        highestY = startingY;
    }

    private void Update()
    {
        // only increase the score when the frog reaches a new height
        if (frog.position.y > highestY)
        {
            highestY = frog.position.y;
        }

        int heightScore = Mathf.FloorToInt((highestY - startingY) * 10f);
        heightText.text = "Height: " + heightScore;
    }
}