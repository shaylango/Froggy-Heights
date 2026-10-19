using UnityEngine;

public class FrogController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D frogBody;
    private Animator frogAnimator;
    private AudioSource frogAudioSource;

    // find the frog's physics body and animation controller
    private void Awake()
    {
        frogBody = GetComponent<Rigidbody2D>();
        frogAnimator = GetComponent<Animator>();
        frogAudioSource = GetComponent<AudioSource>();
    }

    // reads movement input and applies horicontal movement
    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = 1f;
        }

        frogBody.linearVelocity = new Vector2(
            horizontalInput * moveSpeed,
            frogBody.linearVelocity.y
        );

        // wrap the frog around the sides of the screen
        Vector3 position = transform.position;

        if (position.x > 8f)
        {
            position.x = -8f;
        }
        else if (position.x < -8f)
        {
            position.x = 8f;
        }

        transform.position = position;
    }

    // bounces when frog lands on top of platform and side collisions are ignored to avoid getting stuck
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
                    if (frogAnimator != null)
                    {
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
                        // switches the frog to its jump animation after leaving platform
                        private void PlayJumpAnimation() {
                                if (frogAnimator != null) {
                                    frogAnimator.Play("FrogJump", 0, 0f);
                                    }
                                }
}