using UnityEngine;

public class FlyPickup : MonoBehaviour
{
    // points awarded when the frog collects this fly
    [SerializeField] private int pointValue = 10;

    private bool collected;

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

        Destroy(gameObject);
    }
}