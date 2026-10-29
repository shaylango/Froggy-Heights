using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private Transform frog;

    // Fish will not spawn until the frog reaches this height.
    [SerializeField] private float fishStartHeight = 12f;

    // Distance between possible fish spawn heights.
    [SerializeField] private float minimumVerticalGap = 4f;
    [SerializeField] private float maximumVerticalGap = 7f;

    // Horizontal spawn limits.
    [SerializeField] private float horizontalRange = 6f;

    // Minimum distance between a fish and a lily pad.
    [SerializeField] private float minimumDistanceFromPlatforms = 1.5f;

    // How many fish are kept ahead of the frog.
    [SerializeField] private int fishAhead = 3;

    // Removes fish below the camera.
    [SerializeField] private float cleanupDistance = 10f;

    private readonly List<GameObject> spawnedFish =
        new List<GameObject>();

    private float highestFishY;

    private void Start()
    {
        highestFishY = fishStartHeight;
        CreateFishAhead();
    }

    private void Update()
    {
        CreateFishAhead();
        RemoveOldFish();
    }

    private void CreateFishAhead()
    {
        float targetHeight =
            frog.position.y + maximumVerticalGap * fishAhead;

        while (highestFishY < targetHeight)
        {
            SpawnFish();
        }
    }

    private void SpawnFish()
    {
        float verticalGap = Random.Range(
            minimumVerticalGap,
            maximumVerticalGap
        );

        highestFishY += verticalGap;

        for (int attempt = 0; attempt < 20; attempt++)
        {
            float randomX = Random.Range(
                -horizontalRange,
                horizontalRange
            );

            Vector3 spawnPosition = new Vector3(
                randomX,
                highestFishY,
                0f
            );

            if (IsFarEnoughFromPlatforms(spawnPosition))
            {
                GameObject newFish = Instantiate(
                    fishPrefab,
                    spawnPosition,
                    Quaternion.identity
                );

                spawnedFish.Add(newFish);
                return;
            }
        }
    }


    private bool IsFarEnoughFromPlatforms(Vector3 position)
    {
        Collider2D[] nearbyColliders =
            Physics2D.OverlapCircleAll(
                position,
                minimumDistanceFromPlatforms
            );

        foreach (Collider2D nearbyCollider in nearbyColliders)
        {
            if (nearbyCollider.CompareTag("Platform"))
            {
                return false;
            }
        }

        return true;
    }
    
    private void RemoveOldFish()
    {
        float cutoffY =
            Camera.main.transform.position.y - cleanupDistance;

        for (int i = spawnedFish.Count - 1; i >= 0; i--)
        {
            GameObject fish = spawnedFish[i];

            if (fish == null)
            {
                spawnedFish.RemoveAt(i);
            }
            else if (fish.transform.position.y < cutoffY)
            {
                Destroy(fish);
                spawnedFish.RemoveAt(i);
            }
        }
    }
}