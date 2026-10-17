using UnityEngine;

public class FrogController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.5f;
    [SerializeField] private float jumpForce = 10f;

    private Rigidbody2D frogBody;
    private Animator frogAnimator;

    private void Awake()
    {
        frogBody = GetComponent<Rigidbody2D>();
        frogAnimator = GetComponent<Animator>();
    }

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

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.collider.CompareTag("Platform")) {
            return;
            }
            if (frogBody.linearVelocity.y <= 0.1f) {
                if (frogAnimator != null) {
                    frogAnimator.Play("FrogIdle", 0, 0f);
                    }
                    frogBody.linearVelocity = new Vector2(
                        frogBody.linearVelocity.x,
                        jumpForce
                    );
                    
                    CancelInvoke(nameof(PlayJumpAnimation));
                    Invoke(nameof(PlayJumpAnimation), 0.08f);
                    }
                }
                private void PlayJumpAnimation() {
                    if (frogAnimator != null) {
                        frogAnimator.Play("FrogJump", 0, 0f);
                    }
                }
            }