using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    // prefab used to create regular lily pads
    [SerializeField] private GameObject platformPrefab;

    // optional prefab used to create moving lily pads
    [SerializeField] private GameObject movingPlatformPrefab;

    // chance that a generated platform will move
    [SerializeField, Range(0f, 1f)]
    private float movingPlatformChance = 0.25f;

    // frog used to determine when more platforms are needed
    [SerializeField] private Transform frog;

    // highest platform already placed in the scene
    [SerializeField] private float startingHeight = 2.2f;

    // controls the vertical distance between platforms
    [SerializeField] private float minimumVerticalGap = 1.2f;
    [SerializeField] private float maximumVerticalGap = 1.8f;

    // limits where platforms can appear horizontally
    [SerializeField] private float horizontalRange = 3.5f;

    // number of platforms kept ahead of the frog
    [SerializeField] private int platformsAhead = 6;

    // distance below the camera where platforms are removed
    [SerializeField] private float cleanupDistance = 8f;

    // height where moving platforms begin appearing
    [SerializeField] private float movingPlatformsStartHeight = 8f;

    // optional prefab used to create sinking lily pads
    [SerializeField] private GameObject sinkingPlatformPrefab;

    // chance that a generated platform will sink
    [SerializeField, Range(0f, 1f)]
    private float sinkingPlatformChance = 0.2f;

    // height where sinking platforms begin appearing
    [SerializeField] private float sinkingPlatformsStartHeight = 8f;

    // stores platforms currently created by the spawner
    private readonly List<GameObject> spawnedPlatforms =
        new List<GameObject>();

    // highest platform location already checked
    private float highestSpawnedY;

    // sets the starting height and creates platforms ahead
    private void Start()
    {
        highestSpawnedY = startingHeight;
        CreatePlatformsAhead();
    }

    // creates new platforms and removes old ones
    private void Update()
    {
        CreatePlatformsAhead();
        RemoveOldPlatforms();
    }

    // creates enough platforms ahead of the frog
    private void CreatePlatformsAhead()
    {
        float targetHeight =
            frog.position.y +
            maximumVerticalGap *
            platformsAhead;

        while (highestSpawnedY < targetHeight)
        {
            SpawnNextPlatform();
        }
    }

    // randomly creates a regular, moving, or sinking platform
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

        GameObject prefabToSpawn =
            platformPrefab;

        bool canUseSinkingPlatforms =
            highestSpawnedY >=
            startingHeight +
            sinkingPlatformsStartHeight;

        bool canUseMovingPlatforms =
            highestSpawnedY >=
            startingHeight +
            movingPlatformsStartHeight;

        if (canUseSinkingPlatforms &&
            sinkingPlatformPrefab != null &&
            Random.value < sinkingPlatformChance)
        {
            prefabToSpawn =
                sinkingPlatformPrefab;
        }
        else if (canUseMovingPlatforms &&
                 movingPlatformPrefab != null &&
                 Random.value < movingPlatformChance)
        {
            prefabToSpawn =
                movingPlatformPrefab;
        }

        GameObject newPlatform = Instantiate(
            prefabToSpawn,
            spawnPosition,
            Quaternion.identity
        );

        spawnedPlatforms.Add(newPlatform);
    }

    // removes generated platforms below the camera
    private void RemoveOldPlatforms()
    {
        float cutoffY =
            Camera.main.transform.position.y -
            cleanupDistance;

        for (int i = spawnedPlatforms.Count - 1; i >= 0; i--)
        {
            GameObject platform =
                spawnedPlatforms[i];

            if (platform == null)
            {
                spawnedPlatforms.RemoveAt(i);
            }
            else if (platform.transform.position.y < cutoffY)
            {
                Destroy(platform);
                spawnedPlatforms.RemoveAt(i);
            }
        }
    }
}