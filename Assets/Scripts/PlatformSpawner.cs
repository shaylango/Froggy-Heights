using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    // prefab used to create new lily pads
    [SerializeField] private GameObject platformPrefab;

    // the frog determines when more platforms are needed
    [SerializeField] private Transform frog;

    // highest platform already placed in the scene
    [SerializeField] private float startingHeight = 2.2f;

    // controls the vertical distance between platforms
    [SerializeField] private float minimumVerticalGap = 1.2f;
    [SerializeField] private float maximumVerticalGap = 1.8f;

    // limits how far left or right platforms can appear
    [SerializeField] private float horizontalRange = 3.5f;

    // number of platforms kept ahead of the frog
    [SerializeField] private int platformsAhead = 6;

    private float highestSpawnedY;

    private void Start()
    {
        highestSpawnedY = startingHeight;

        CreatePlatformsAhead();
    }

    private void Update()
    {
        // add more platforms as the frog climbs
        CreatePlatformsAhead();
    }

    private void CreatePlatformsAhead()
    {
        float targetHeight =
            frog.position.y + maximumVerticalGap * platformsAhead;

        while (highestSpawnedY < targetHeight)
        {
            SpawnNextPlatform();
        }
    }

    private void SpawnNextPlatform()
    {
        float verticalGap = Random.Range(
            minimumVerticalGap,
            maximumVerticalGap
        );

        highestSpawnedY += verticalGap;

        float randomX = Random.Range(
            -horizontalRange,
            horizontalRange
        );

        Vector3 spawnPosition = new Vector3(
            randomX,
            highestSpawnedY,
            0f
        );

        Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
    }
}