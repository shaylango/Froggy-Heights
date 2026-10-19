using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // how far the platform moves from its starting position
    [SerializeField] private float moveDistance = 2f;

    // how quickly the platform moves
    [SerializeField] private float moveSpeed = 1.5f;

    private Vector3 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        // moves the platform left and right continuously
        float horizontalOffset =
            Mathf.PingPong(Time.time * moveSpeed, moveDistance * 2f)
            - moveDistance;

        transform.position = new Vector3(
            startingPosition.x + horizontalOffset,
            startingPosition.y,
            startingPosition.z
        );
    }
}