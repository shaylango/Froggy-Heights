using System.Collections;
using UnityEngine;

public class SinkingPlatform : MonoBehaviour
{
    // time before the platform starts sinking
    [SerializeField] private float sinkDelay = 0.35f;

    // length of the sinking animation
    [SerializeField] private float sinkDuration = 0.4f;

    // distance the platform moves downward
    [SerializeField] private float sinkDistance = 1.2f;

    // sound played when the platform starts sinking
    [SerializeField] private AudioClip sinkingSound;

    [SerializeField, Range(0f, 1f)]
    private float sinkingVolume = 0.7f;

    private AudioSource platformAudioSource;
    private Collider2D platformCollider;
    private SpriteRenderer platformRenderer;
    private bool isSinking;

    // finds the platform's components
    private void Awake()
    {
        platformCollider =
            GetComponent<Collider2D>();

        platformRenderer =
            GetComponent<SpriteRenderer>();

        platformAudioSource =
            GetComponent<AudioSource>();
    }

    // checks whether the frog landed on top of the platform
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isSinking)
        {
            return;
        }

        FrogController frog =
            collision.collider.GetComponent<FrogController>();

        if (frog == null ||
            platformCollider == null)
        {
            return;
        }

        Rigidbody2D frogBody =
            frog.GetComponent<Rigidbody2D>();

        // only starts sinking while the frog is falling
        if (frogBody == null ||
            frogBody.linearVelocity.y > 0.1f)
        {
            return;
        }

        float frogBottom =
            collision.collider.bounds.min.y;

        float platformTop =
            platformCollider.bounds.max.y;

        // ignores side contact and checks for a top landing
        if (frogBottom >= platformTop - 0.15f)
        {
            StartCoroutine(
                SinkPlatform()
            );
        }
    }

    // moves the platform downward and removes it
    private IEnumerator SinkPlatform()
    {
        isSinking = true;

        if (platformAudioSource != null &&
            sinkingSound != null)
        {
            platformAudioSource.PlayOneShot(
                sinkingSound,
                sinkingVolume
            );
        }

        yield return new WaitForSeconds(
            sinkDelay
        );

        platformCollider.enabled = false;

        Vector3 startingPosition =
            transform.position;

        Vector3 endingPosition =
            startingPosition +
            Vector3.down *
            sinkDistance;

        float timer = 0f;

        while (timer < sinkDuration)
        {
            timer += Time.deltaTime;

            float progress =
                timer / sinkDuration;

            transform.position =
                Vector3.Lerp(
                    startingPosition,
                    endingPosition,
                    progress
                );

            yield return null;
        }

        if (platformRenderer != null)
        {
            platformRenderer.enabled = false;
        }

        Destroy(gameObject);
    }
}