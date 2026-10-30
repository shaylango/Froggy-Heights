using UnityEngine;

public class BirdHazard : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // checks if the frog touched the bird
        if (other.GetComponent<FrogController>() == null)
        {
            return;
        }

        // finds the game manager and triggers game over
        GameManager gameManager =
            FindAnyObjectByType<GameManager>();

        if (gameManager != null)
        {
            gameManager.TriggerHazardGameOver();
        }
    }
}