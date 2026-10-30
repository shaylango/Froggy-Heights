using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    // prefab used to create fish
    [SerializeField] private GameObject fishPrefab;

    // frog used to determine when fish should spawn
    [SerializeField] private Transform frog;

    // height where fish begin appearing
    [SerializeField] private float fishStartHeight = 12f;

    // controls the vertical distance between fish
    [SerializeField] private float minimumVerticalGap = 4f;
    [SerializeField] private float maximumVerticalGap = 7f;

    // limits where fish can spawn horizontally
    [SerializeField] private float horizontalRange = 6f;

    // minimum distance between fish and lily pads
    [SerializeField] private float minimumDistanceFromPlatforms = 1.5f;

    // number of possible fish locations kept ahead of the frog
    [SerializeField] private int fishAhead = 3;

    // distance below the camera where fish are removed
    [SerializeField] private float cleanupDistance = 10f;

    // stores fish currently created by the spawner
    private readonly List<GameObject> spawnedFish =
        new List<GameObject>();

    // highest possible fish location already checked
    private float highestFishY;

    // sets the first fish height and creates fish ahead
    private void Start()
    {
        highestFishY = fishStartHeight;
        CreateFishAhead();
    }

    // creates new fish and removes old ones
    private void Update()
    {
        CreateFishAhead();
        RemoveOldFish();
    }

    // creates enough possible fish locations ahead of the frog
    private void CreateFishAhead()
    {
        float targetHeight =
            frog.position.y +
            maximumVerticalGap *
            fishAhead;

        while (highestFishY < targetHeight)
        {
            SpawnFish();
        }
    }

    // tries several positions before creating a fish
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

    // checks whether a fish is far enough from nearby lily pads
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

    // removes fish that are far below the camera
    private void RemoveOldFish()
    {
        float cutoffY =
            Camera.main.transform.position.y -
            cleanupDistance;

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