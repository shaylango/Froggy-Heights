using System.Collections.Generic;
using UnityEngine;

public class FlySpawner : MonoBehaviour
{
    // prefabs used for regular and golden flies
    [SerializeField] private GameObject regularFlyPrefab;
    [SerializeField] private GameObject goldenFlyPrefab;

    // frog used to determine when new flies are needed
    [SerializeField] private Transform frog;

    // controls the vertical distance between flies
    [SerializeField] private float minimumVerticalGap = 2.5f;
    [SerializeField] private float maximumVerticalGap = 4f;

    // limits where flies can spawn horizontally
    [SerializeField] private float horizontalRange = 6f;

    // number of possible fly locations kept ahead of the frog
    [SerializeField] private int fliesAhead = 5;

    // chance that a spawned fly is golden
    [SerializeField, Range(0f, 1f)]
    private float goldenFlyChance = 0.15f;

    // distance below the camera where flies are removed
    [SerializeField] private float cleanupDistance = 8f;

    // stores flies currently created by the spawner
    private readonly List<GameObject> spawnedFlies =
        new List<GameObject>();

    // highest possible fly location already checked
    private float highestSpawnedY;

    // sets the starting height and creates flies ahead
    private void Start()
    {
        highestSpawnedY = -3.5f;
        CreateFliesAhead();
    }

    // creates new flies and removes old ones
    private void Update()
    {
        CreateFliesAhead();
        RemoveOldFlies();
    }

    // creates enough possible fly locations ahead of the frog
    private void CreateFliesAhead()
    {
        float targetHeight =
            frog.position.y +
            maximumVerticalGap *
            fliesAhead;

        while (highestSpawnedY < targetHeight)
        {
            SpawnNextFly();
        }
    }

    // randomly creates a regular or golden fly
    private void SpawnNextFly()
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
            regularFlyPrefab;

        if (goldenFlyPrefab != null &&
            Random.value < goldenFlyChance)
        {
            prefabToSpawn = goldenFlyPrefab;
        }

        GameObject newFly = Instantiate(
            prefabToSpawn,
            spawnPosition,
            Quaternion.identity
        );

        spawnedFlies.Add(newFly);
    }

    // removes generated flies below the camera
    private void RemoveOldFlies()
    {
        float cutoffY =
            Camera.main.transform.position.y -
            cleanupDistance;

        for (int i = spawnedFlies.Count - 1; i >= 0; i--)
        {
            GameObject fly = spawnedFlies[i];

            if (fly == null)
            {
                spawnedFlies.RemoveAt(i);
            }
            else if (fly.transform.position.y < cutoffY)
            {
                Destroy(fly);
                spawnedFlies.RemoveAt(i);
            }
        }
    }
}