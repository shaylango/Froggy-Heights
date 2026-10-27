using System.Collections;
using UnityEngine;

public class FrogController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private float tongueRange = 3f;
    private Transform tongueVisual;
    private SpriteRenderer tongueRenderer;
    private Rigidbody2D frogBody;
    private Animator frogAnimator;
    private AudioSource frogAudioSource;

    // finds the frog's physics body, animator, audio source, and tongue
    private void Awake() {
        frogBody = GetComponent<Rigidbody2D>();
        frogAnimator = GetComponent<Animator>();
        frogAudioSource = GetComponent<AudioSource>();
        tongueVisual = transform.Find("TongueVisual");

        if (tongueVisual != null) {
            tongueRenderer = tongueVisual.GetComponent<SpriteRenderer>();
            tongueVisual.gameObject.SetActive(false);
        }
    }

    // reads movement input and applies horizontal movement
    private void Update() {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.LeftArrow)) {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D) ||
                 Input.GetKey(KeyCode.RightArrow)) {
            horizontalInput = 1f;
        }

        frogBody.linearVelocity = new Vector2(
            horizontalInput * moveSpeed,
            frogBody.linearVelocity.y
        );

        // wraps the frog around the sides of the screen
        Vector3 position = transform.position;

        if (position.x > 8f) {
            position.x = -8f;
        }
        else if (position.x < -8f) {
            position.x = 8f;
        }

        transform.position = position;

        // uses the tongue when Space is pressed
        if (Input.GetKeyDown(KeyCode.Space)) {
            TryUseTongue();
        }
    }

    // bounces when the frog lands on top of a platform
    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.collider.CompareTag("Platform")) {
            return;
        }

        bool landedOnTop = false;

        for (int i = 0; i < collision.contactCount; i++) {
            ContactPoint2D contact = collision.GetContact(i);

            if (contact.normal.y > 0.5f) {
                landedOnTop = true;
                break;
            }
        }

        if (!landedOnTop || frogBody.linearVelocity.y > 0.1f) {
            return;
        }

        if (frogAnimator != null) {
            frogAnimator.Play("FrogIdle", 0, 0f);
        }

        frogBody.linearVelocity = new Vector2(
            frogBody.linearVelocity.x,
            jumpForce
        );

        if (frogAudioSource != null && jumpSound != null) {
            frogAudioSource.PlayOneShot(jumpSound);
        }

        CancelInvoke(nameof(PlayJumpAnimation));
        Invoke(nameof(PlayJumpAnimation), 0.08f);
    }

    // switches the frog to its jump animation
    private void PlayJumpAnimation() {
        if (frogAnimator != null) {
            frogAnimator.Play("FrogJump", 0, 0f);
        }
    }

    // finds the closest fly within tongue range
    private void TryUseTongue() {
        Collider2D[] nearbyObjects =
            Physics2D.OverlapCircleAll(
                transform.position,
                tongueRange
            );

        FlyPickup closestFly = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D nearbyObject in nearbyObjects) {
            FlyPickup fly =
                nearbyObject.GetComponent<FlyPickup>();

            if (fly == null) {
                continue;
            }

            float distance = Vector2.Distance(
                transform.position,
                fly.transform.position
            );

            if (distance < closestDistance) {
                closestDistance = distance;
                closestFly = fly;
            }
        }

        if (closestFly != null) {
            StartCoroutine(UseTongue(closestFly));
        }
    }

    // displays the tongue briefly between the frog and the fly
    private IEnumerator UseTongue(FlyPickup fly) {
        if (tongueVisual == null ||
            tongueRenderer == null ||
            tongueRenderer.sprite == null) {
            fly.Collect();
            yield break;
        }

        Vector3 startPosition = tongueVisual.position;
        Vector3 endPosition = fly.transform.position;

        Vector2 direction = endPosition - startPosition;
        float distance = Mathf.Min(
        direction.magnitude,
        tongueRange
        );

        tongueVisual.position = startPosition;
        tongueVisual.right = direction.normalized;

        tongueVisual.localScale = Vector3.one;
        float parentScale = Mathf.Max(
            tongueVisual.lossyScale.x,
            0.001f
        );

        tongueRenderer.size = new Vector2(
            distance / parentScale,
            0.02f
        );

        tongueVisual.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.12f);

        fly.Collect();

        yield return new WaitForSeconds(0.05f);

        tongueVisual.gameObject.SetActive(false);
    }
}