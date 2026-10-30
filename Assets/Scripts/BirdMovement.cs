using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    // controls how quickly the bird moves
    [SerializeField] private float moveSpeed = 2.5f;

    // limits the bird's horizontal movement
    [SerializeField] private float leftLimit = -7f;
    [SerializeField] private float rightLimit = 7f;

    // controls the bird's starting direction
    [SerializeField] private float movementDirection = 1f;

    // sound played while the bird is visible
    [SerializeField] private AudioClip birdSound;

    [SerializeField, Range(0f, 1f)]
    private float birdVolume = 0.5f;

    private AudioSource birdAudioSource;
    private SpriteRenderer birdRenderer;

    // finds the bird's audio source and sprite renderer
    private void Awake()
    {
        birdRenderer =
            GetComponentInChildren<SpriteRenderer>();

        birdAudioSource =
            GetComponent<AudioSource>();
    }

    // sets the bird's starting direction
    private void Start()
    {
        UpdateFacingDirection();
    }

    // starts the bird sound when it enters the camera view
    private void OnBecameVisible()
    {
        if (birdAudioSource != null &&
            birdSound != null &&
            !birdAudioSource.isPlaying)
        {
            birdAudioSource.clip = birdSound;
            birdAudioSource.volume = birdVolume;
            birdAudioSource.loop = true;
            birdAudioSource.Play();
        }
    }

    // moves the bird and reverses it at the screen limits
    private void Update()
    {
        transform.position +=
            Vector3.right *
            movementDirection *
            moveSpeed *
            Time.deltaTime;

        if (transform.position.x >= rightLimit)
        {
            movementDirection = -1f;
            UpdateFacingDirection();
        }
        else if (transform.position.x <= leftLimit)
        {
            movementDirection = 1f;
            UpdateFacingDirection();
        }
    }

    // flips the bird to match its movement direction
    private void UpdateFacingDirection()
    {
        if (birdRenderer != null)
        {
            birdRenderer.flipX =
                movementDirection < 0f;
        }
    }
}