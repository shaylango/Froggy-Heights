using UnityEngine;

public class FishHazard : MonoBehaviour
{
    // sound played when the fish enters the camera view
    [SerializeField] private AudioClip fishSound;

    [SerializeField, Range(0f, 1f)]
    private float fishVolume = 0.5f;

    private AudioSource fishAudioSource;
    private bool soundPlayed;

    // finds the fish audio source
    private void Awake()
    {
        fishAudioSource =
            GetComponent<AudioSource>();
    }

    // plays the fish sound once when it becomes visible
    private void OnBecameVisible()
    {
        if (soundPlayed)
        {
            return;
        }

        if (fishAudioSource != null &&
            fishSound != null)
        {
            fishAudioSource.PlayOneShot(
                fishSound,
                fishVolume
            );

            soundPlayed = true;
        }
    }

    // triggers game over when the frog touches the fish
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<FrogController>() == null)
        {
            return;
        }

        GameManager gameManager =
            FindAnyObjectByType<GameManager>();

        if (gameManager != null)
        {
            gameManager.TriggerHazardGameOver();
        }
    }
}