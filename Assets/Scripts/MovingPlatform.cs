using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // left and right movement limits.
    [SerializeField] private float leftLimit = -7f;
    [SerializeField] private float rightLimit = 7f;

    // how quickly the platform moves
    [SerializeField] private float moveSpeed = 1.5f;

    private float movementOffset;
    private float movementDirection;

    private void Start()
    {
        // gives each platform a different starting point
        movementOffset = Random.Range(
            0f,
            rightLimit - leftLimit
        );

        // gives each platform a random direction
        movementDirection = Random.value < 0.5f ? -1f : 1f;
    }

    private void Update()
    {
        // creates independent movement for each platform
        float movementTime =
            Time.time * moveSpeed * movementDirection
            + movementOffset;

        float horizontalPosition =
            Mathf.PingPong(
                movementTime,
                rightLimit - leftLimit
            ) + leftLimit;

        transform.position = new Vector3(
            horizontalPosition,
            transform.position.y,
            transform.position.z
        );
    }
}