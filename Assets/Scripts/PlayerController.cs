using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float acceleration = 12f;
    public float deceleration = 16f;
    public float rotationSpeed = 12f;

    [Header("Jump")]
    public float jumpForce = 7f;

    [Header("Ground Detection")]
    public LayerMask groundLayer = ~0;
    public float groundCheckDistance = 0.08f;

    [Header("Animation")]
    public Animator animator;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private Camera mainCamera;

    private bool isGrounded;

    private float currentHorizontal;
    private float currentVertical;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        mainCamera = Camera.main;

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Rigidbody
        rb.useGravity = true;
        rb.isKinematic = false;

        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezeRotationZ;

        rb.collisionDetectionMode =
            CollisionDetectionMode.ContinuousDynamic;

        rb.interpolation =
            RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        CheckGround();

        if (Keyboard.current == null)
            return;

        // Jump
        if (Keyboard.current.spaceKey.wasPressedThisFrame &&
            isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (Keyboard.current == null)
            return;

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // ==========================================
        // INPUT
        // ==========================================

        float targetHorizontal = 0f;
        float targetVertical = 0f;

        if (Keyboard.current.leftArrowKey.isPressed)
            targetHorizontal = -1f;

        if (Keyboard.current.rightArrowKey.isPressed)
            targetHorizontal = 1f;

        if (Keyboard.current.upArrowKey.isPressed)
            targetVertical = 1f;

        if (Keyboard.current.downArrowKey.isPressed)
            targetVertical = -1f;

        // ==========================================
        // SMOOTH INPUT
        // ==========================================

        float smoothSpeed =
            (Mathf.Abs(targetHorizontal) > 0.01f ||
             Mathf.Abs(targetVertical) > 0.01f)
            ? acceleration
            : deceleration;

        currentHorizontal = Mathf.MoveTowards(
            currentHorizontal,
            targetHorizontal,
            smoothSpeed * Time.fixedDeltaTime
        );

        currentVertical = Mathf.MoveTowards(
            currentVertical,
            targetVertical,
            smoothSpeed * Time.fixedDeltaTime
        );

        // ==========================================
        // CAMERA DIRECTIONS
        // ==========================================

        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // ==========================================
        // MOVEMENT
        // ==========================================

        Vector3 movement =
            forward * currentVertical +
            right * currentHorizontal;

        if (movement.sqrMagnitude > 1f)
        {
            movement.Normalize();
        }

        // ==========================================
        // SMOOTH PHYSICS MOVEMENT
        // ==========================================

        Vector3 targetVelocity =
            movement * moveSpeed;

        Vector3 velocity = rb.linearVelocity;

        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;

        // Keep gravity/jump velocity
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;

        // ==========================================
        // ANIMATION
        // ==========================================

        if (animator != null)
        {
            animator.SetFloat(
                "Horizontal",
                currentHorizontal
            );

            animator.SetFloat(
                "Vertical",
                currentVertical
            );

            animator.SetBool(
                "IsGrounded",
                isGrounded
            );
        }

        // ==========================================
        // CHARACTER ROTATION
        // ==========================================

        if (movement.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(movement)
                * Quaternion.Euler(0f, 180f, 0f);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );
        }
    }

    // ==========================================
    // JUMP
    // ==========================================

    void Jump()
    {
        Vector3 velocity = rb.linearVelocity;

        velocity.y = 0f;

        rb.linearVelocity = velocity;

        rb.AddForce(
            Vector3.up * jumpForce,
            ForceMode.Impulse
        );

        isGrounded = false;

        if (animator != null)
        {
            animator.SetBool("IsJumping", true);
        }
    }

    // ==========================================
    // GROUND CHECK
    // ==========================================

    void CheckGround()
    {
        Bounds bounds = capsuleCollider.bounds;

        // Start slightly above the bottom of the capsule
        Vector3 origin = new Vector3(
            bounds.center.x,
            bounds.min.y + 0.05f,
            bounds.center.z
        );

        float radius =
            Mathf.Min(
                bounds.extents.x,
                bounds.extents.z
            ) * 0.8f;

        isGrounded = Physics.CheckSphere(
            origin,
            radius + groundCheckDistance,
            groundLayer,
            QueryTriggerInteraction.Ignore
        );

        // Don't allow jump animation to stay forever
        if (isGrounded && animator != null)
        {
            animator.SetBool("IsJumping", false);
        }
    }
}