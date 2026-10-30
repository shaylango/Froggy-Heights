using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // limits where the platform can move horizontally
    [SerializeField] private float leftLimit = -7f;
    [SerializeField] private float rightLimit = 7f;

    // controls how quickly the platform moves
    [SerializeField] private float moveSpeed = 1.5f;

    // gives each platform different movement timing
    private float movementOffset;

    // gives each platform a different starting direction
    private float movementDirection;

    // sets a random movement offset and direction
    private void Start()
    {
        movementOffset = Random.Range(
            0f,
            rightLimit - leftLimit
        );

        movementDirection =
            Random.value < 0.5f
                ? -1f
                : 1f;
    }

    // moves the platform between the horizontal limits
    private void Update()
    {
        float movementTime =
            Time.time *
            moveSpeed *
            movementDirection +
            movementOffset;

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