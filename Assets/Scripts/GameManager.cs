using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // The frog being watched for falling.
    [SerializeField] private Transform frog;

    // The message shown after the frog loses.
    [SerializeField] private GameObject gameOverText;

    // How far below the camera the frog can fall.
    [SerializeField] private float fallDistance = 6f;

    private bool gameOver;

    private void Start()
    {
        gameOverText.SetActive(false);
    }

    private void Update()
    {
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(
                    SceneManager.GetActiveScene().name
                );
            }

            return;
        }

        // End the game when the frog falls below the camera.
        if (frog.position.y < Camera.main.transform.position.y - fallDistance)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        gameOver = true;

        Rigidbody2D frogBody = frog.GetComponent<Rigidbody2D>();

        if (frogBody != null)
        {
            frogBody.linearVelocity = Vector2.zero;
        }

        frog.gameObject.SetActive(false);
        gameOverText.SetActive(true);
    }
}