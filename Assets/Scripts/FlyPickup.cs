using UnityEngine;

public class FlyPickup : MonoBehaviour
{
    // points awarded when the frog collects this fly
    [SerializeField] private int pointValue = 10;

    // sound played when the fly is collected
    [SerializeField] private AudioClip collectionSound;

    [SerializeField, Range(0f, 1f)]
    private float collectionVolume = 0.8f;

    private AudioSource flyAudioSource;
    private bool collected;

    // finds the fly's audio source
    private void Awake()
    {
        flyAudioSource =
            GetComponent<AudioSource>();
    }

    // collects the fly when the frog touches it
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<FrogController>() != null)
        {
            Collect();
        }
    }

    // allows the frog's tongue to collect the fly
    public void Collect()
    {
        if (collected)
        {
            return;
        }

        collected = true;

        // adds the fly's points to the score
        HeightScore score =
            FindAnyObjectByType<HeightScore>();

        if (score != null)
        {
            score.AddPoints(pointValue);
        }

        if (flyAudioSource != null &&
            collectionSound != null)
        {
            Collider2D flyCollider =
                GetComponent<Collider2D>();

            if (flyCollider != null)
            {
                flyCollider.enabled = false;
            }

            SpriteRenderer flyRenderer =
                GetComponent<SpriteRenderer>();

            if (flyRenderer != null)
            {
                flyRenderer.enabled = false;
            }

            flyAudioSource.PlayOneShot(
                collectionSound,
                collectionVolume
            );

            // keeps the object alive until the sound finishes
            Destroy(
                gameObject,
                collectionSound.length
            );
        }
        else
        {
            Destroy(gameObject);
        }
    }
}