using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // the frog being watched for falling
    [SerializeField] private Transform frog;

    // the message shown after the frog loses
    [SerializeField] private GameObject gameOverText;

    // how far below the camera the frog can fall
    [SerializeField] private float fallDistance = 6f;

    [SerializeField] private AudioClip gameOverSound;

    private bool gameOver;
    private AudioSource gameManagerAudioSource;

    private void Start()
    {
        gameOverText.SetActive(false);
        gameManagerAudioSource = GetComponent<AudioSource>();
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

        // end the game when the frog falls below the camera
        if (frog.position.y < Camera.main.transform.position.y - fallDistance)
        {
            EndGame();
            if (gameManagerAudioSource != null && gameOverSound != null) {
                gameManagerAudioSource.PlayOneShot(gameOverSound);
            }
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