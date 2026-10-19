using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    // prefab used to create new lily pads
    [SerializeField] private GameObject platformPrefab;

    // optional prefab used for moving lily pads
    [SerializeField] private GameObject movingPlatformPrefab;

    // chance that a generated platform will move
    [SerializeField, Range(0f, 1f)]
    private float movingPlatformChance = 0.25f;

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

    // stores platforms created by the spawner
    private List<GameObject> spawnedPlatforms =
    new List<GameObject>();

    // how far below the camera a platform can remain
    [SerializeField] private float cleanupDistance = 8f;

    // height above the starting area where moving platforms begin
    [SerializeField] private float movingPlatformsStartHeight = 8f;

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
        RemoveOldPlatforms();
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

        GameObject prefabToSpawn = platformPrefab;

        if (movingPlatformPrefab != null &&
            highestSpawnedY >= startingHeight + movingPlatformsStartHeight &&
            Random.value < movingPlatformChance) {
            prefabToSpawn = movingPlatformPrefab;
        }

        GameObject newPlatform = Instantiate(
            prefabToSpawn,
            spawnPosition,
            Quaternion.identity
        );

        spawnedPlatforms.Add(newPlatform);
    }
    // removes generated platforms that are far below the camera
    private void RemoveOldPlatforms() {
        float cutoffY = Camera.main.transform.position.y
        - cleanupDistance;

        for (int i = spawnedPlatforms.Count - 1; i >= 0; i--) {
            GameObject platform = spawnedPlatforms[i];
            if (platform == null) {
                spawnedPlatforms.RemoveAt(i);
            }
            else if (platform.transform.position.y < cutoffY) {
                Destroy(platform);
                spawnedPlatforms.RemoveAt(i);
            }
        }
    }
}