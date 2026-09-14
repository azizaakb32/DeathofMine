using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public float crouchSpeed = 2f;

    [Header("Jump")]
    public float jumpHeight = 1.5f;

    [Header("Gravity")]
    public float gravity = -20f;

    [Header("Crouch")]
    public float standingHeight = 2f;
    public float crouchHeight = 1f;

    [Header("Footsteps")]
    public AudioClip[] footstepSounds;
    public float walkStepRate = 0.5f;
    public float sprintStepRate = 0.35f;
    public float crouchStepRate = 0.8f;

    private CharacterController controller;
    private AudioSource audioSource;
    private Vector3 velocity;
    private float stepTimer;

    // True when the player is actually pressing a movement key
    private bool isTryingToMove;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        audioSource.loop = false;
        audioSource.playOnAwake = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Move();
        HandleCrouch();
        HandleFootsteps();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // True when the player is pressing WASD or arrow keys
        isTryingToMove = move.sqrMagnitude > 0.01f;

        float currentSpeed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
            currentSpeed = sprintSpeed;

        if (Input.GetKey(KeyCode.LeftControl))
            currentSpeed = crouchSpeed;

        controller.Move(move * currentSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleCrouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            controller.height = crouchHeight;
        }
        else
        {
            controller.height = standingHeight;
        }
    }

    void HandleFootsteps()
    {
        // 1. Mute footsteps if the player is airborne or not moving
        if (!controller.isGrounded || !isTryingToMove)
        {
            stepTimer = 0f;
            // Only cut the audio when the player has actually stopped
            if (!isTryingToMove && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            return;
        }

        // 2. Handle the case where the player is pressed against a wall
        // (key held down but not actually moving). The CharacterController's
        // velocity drops to near zero when it collides with a wall.
        Vector3 actualHorizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        if (actualHorizontalVelocity.magnitude < 0.2f)
        {
            stepTimer = 0f;
            return;
        }

        // 3. Pick the step rate based on current speed
        float stepRate = walkStepRate;

        if (Input.GetKey(KeyCode.LeftShift))
            stepRate = sprintStepRate;

        if (Input.GetKey(KeyCode.LeftControl))
            stepRate = crouchStepRate;

        stepTimer += Time.deltaTime;

        if (stepTimer >= stepRate)
        {
            PlayFootstep();
            stepTimer = 0f;
        }
    }

    void PlayFootstep()
    {
        if (footstepSounds == null || footstepSounds.Length == 0)
            return;

        // Skip if the previous footstep clip hasn't finished yet, so sounds don't stack/overlap
        if (audioSource.isPlaying)
            return;

        AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
        audioSource.PlayOneShot(clip);
    }
}