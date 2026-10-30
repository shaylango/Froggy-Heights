using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // frog being watched for falling
    [SerializeField] private Transform frog;

    // message shown after the player loses
    [SerializeField] private GameObject gameOverText;

    // distance below the camera where the game ends
    [SerializeField] private float fallDistance = 6f;

    // sound played when the game ends
    [SerializeField] private AudioClip gameOverSound;

    private bool gameOver;
    private AudioSource gameManagerAudioSource;

    // sets up the game over screen and audio source
    private void Start()
    {
        gameOverText.SetActive(false);
        gameManagerAudioSource =
            GetComponent<AudioSource>();
    }

    // checks for falling and restart input
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

        // ends the game when the frog falls below the camera
        if (frog.position.y <
            Camera.main.transform.position.y -
            fallDistance)
        {
            EndGame();
            PlayGameOverSound();
        }
    }

    // stops the frog and displays the game over message
    private void EndGame()
    {
        gameOver = true;

        Rigidbody2D frogBody =
            frog.GetComponent<Rigidbody2D>();

        if (frogBody != null)
        {
            frogBody.linearVelocity =
                Vector2.zero;
        }

        frog.gameObject.SetActive(false);
        gameOverText.SetActive(true);
    }

    // triggers game over when the frog touches a hazard
    public void TriggerHazardGameOver()
    {
        if (gameOver)
        {
            return;
        }

        EndGame();
        PlayGameOverSound();
    }

    // plays the game over sound
    private void PlayGameOverSound()
    {
        if (gameManagerAudioSource != null &&
            gameOverSound != null)
        {
            gameManagerAudioSource.PlayOneShot(
                gameOverSound
            );
        }
    }
}