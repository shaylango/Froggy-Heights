using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // frog that the camera follows
    [SerializeField] private Transform target;

    // controls how smoothly the camera moves upward
    [SerializeField] private float followSpeed = 5f;

    // moves the camera upward without moving it downward
    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        float desiredY = Mathf.Max(
            transform.position.y,
            target.position.y
        );

        float newY = Mathf.Lerp(
            transform.position.y,
            desiredY,
            followSpeed * Time.deltaTime
        );

        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );
    }
}