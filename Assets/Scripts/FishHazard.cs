using UnityEngine;

public class FishHazard : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<FrogController>() != null)
        {
            GameManager gameManager =
                FindAnyObjectByType<GameManager>();

            if (gameManager != null)
            {
                gameManager.TriggerHazardGameOver();
            }
        }
    }
}