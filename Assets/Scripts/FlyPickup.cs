using UnityEngine;

public class FlyPickup : MonoBehaviour
{
    // points awarded when the frog collects this flyp
    [SerializeField] private int pointValue = 10;

    // sound played when the fly is collected
    [SerializeField] private AudioClip collectionSound;

    [SerializeField, Range(0f, 1f)]
    private float collectionVolume = 0.8f;

    private AudioSource flyAudioSource;
    private bool collected;

    private void Awake()
    {
        flyAudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Touching the fly with the frog collects it.
        if (other.GetComponent<FrogController>() != null)
        {
            Collect();
        }
    }

    // allows the frog's tongue to collect the fly from a distance
    public void Collect()
    {
        if (collected)
        {
            return;
        }

        collected = true;

        HeightScore score = FindAnyObjectByType<HeightScore>();

        if (score != null)
        {
            score.AddPoints(pointValue);
        }

        if (flyAudioSource != null && collectionSound != null)
        {
            Collider2D flyCollider = GetComponent<Collider2D>();

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

            Destroy(gameObject, collectionSound.length);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}