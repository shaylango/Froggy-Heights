using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    // prefabs used for birds facing each direction
    [SerializeField] private GameObject birdRightPrefab;
    [SerializeField] private GameObject birdLeftPrefab;

    // frog used to determine when birds should spawn
    [SerializeField] private Transform frog;

    // height where birds begin appearing
    [SerializeField] private float birdStartHeight = 25f;

    // controls the vertical distance between birds
    [SerializeField] private float minimumVerticalGap = 7f;
    [SerializeField] private float maximumVerticalGap = 10f;

    // limits where birds can spawn horizontally
    [SerializeField] private float horizontalRange = 6f;

    // number of possible bird locations kept ahead of the frog
    [SerializeField] private int birdsAhead = 2;

    // distance below the camera where birds are removed
    [SerializeField] private float cleanupDistance = 12f;

    // chance that a possible bird location spawns a bird
    [SerializeField, Range(0f, 1f)]
    private float birdSpawnChance = 0.25f;

    // minimum vertical distance between birds and fish
    [SerializeField] private float minimumDistanceFromFish = 3f;

    // stores birds currently created by the spawner
    private readonly List<GameObject> spawnedBirds =
        new List<GameObject>();

    // highest possible bird location already checked
    private float highestBirdY;

    // sets the first bird height and creates birds ahead
    private void Start()
    {
        highestBirdY = birdStartHeight;
        CreateBirdsAhead();
    }

    // creates new birds and removes old ones
    private void Update()
    {
        CreateBirdsAhead();
        RemoveOldBirds();
    }

    // creates enough possible bird locations ahead of the frog
    private void CreateBirdsAhead()
    {
        float targetHeight =
            frog.position.y +
            maximumVerticalGap *
            birdsAhead;

        while (highestBirdY < targetHeight)
        {
            SpawnBird();
        }
    }

    // randomly creates a bird at the next available height
    private void SpawnBird()
    {
        float verticalGap = Random.Range(
            minimumVerticalGap,
            maximumVerticalGap
        );

        highestBirdY += verticalGap;

        // leaves most possible bird locations empty
        if (Random.value > birdSpawnChance)
        {
            return;
        }

        // prevents birds from appearing too close to fish
        if (IsTooCloseToFish(highestBirdY))
        {
            return;
        }

        float randomX = Random.Range(
            -horizontalRange,
            horizontalRange
        );

        Vector3 spawnPosition = new Vector3(
            randomX,
            highestBirdY,
            0f
        );

        GameObject prefabToSpawn =
            Random.value < 0.5f
                ? birdRightPrefab
                : birdLeftPrefab;

        GameObject newBird = Instantiate(
            prefabToSpawn,
            spawnPosition,
            Quaternion.identity
        );

        spawnedBirds.Add(newBird);
    }

    // checks whether a bird would be too close to an existing fish
    private bool IsTooCloseToFish(float birdHeight)
    {
        FishHazard[] fishHazards =
            FindObjectsByType<FishHazard>();

        foreach (FishHazard fish in fishHazards)
        {
            float heightDifference =
                Mathf.Abs(
                    fish.transform.position.y -
                    birdHeight
                );

            if (heightDifference < minimumDistanceFromFish)
            {
                return true;
            }
        }

        return false;
    }

    // removes birds that are far below the camera
    private void RemoveOldBirds()
    {
        float cutoffY =
            Camera.main.transform.position.y -
            cleanupDistance;

        for (int i = spawnedBirds.Count - 1; i >= 0; i--)
        {
            GameObject bird = spawnedBirds[i];

            if (bird == null)
            {
                spawnedBirds.RemoveAt(i);
            }
            else if (bird.transform.position.y < cutoffY)
            {
                Destroy(bird);
                spawnedBirds.RemoveAt(i);
            }
        }
    }
}