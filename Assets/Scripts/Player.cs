using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [HideInInspector] public bool playerUnlocked;

    [Header("Death info")]
    [SerializeField] private Vector2 knockbackDirection;
    private bool isDead;

    [Header("Movement info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedMult;
    private float defaultMoveSpeed;
    [Space]
    [SerializeField] private float milestoneIncreaser;
    private float defaultMilestoneIncreaser;
    private float speedMilestone;


    [Header("Jump info")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpPower;
    private bool DoubleJumpAvailable;

    [Header("Sliding info")]
    [SerializeField] private float slidingSpeed;
    [SerializeField] private float slidingTime;
    [SerializeField] private float slidingCooldown;
    private float slidingCooldownCounter;
    private float slidingTimerCounter;
    private bool isSliding;

    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float ceilingCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallDetection;
    [SerializeField] private Vector2 wallDetectionSize;
    private bool isGrounded;
    private bool ceilingDetection;
    private bool wallDetected;

    void Start()
    {
        // Get component references
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Set initial speed milestone
        speedMilestone = milestoneIncreaser;
        // Store default speed values for resetting
        defaultMoveSpeed = moveSpeed;
        defaultMilestoneIncreaser = milestoneIncreaser;
    }

    void Update()
    {
        // Check for collisions with ground, ceiling, and walls
        CheckCollision();
        // Update animator parameters
        AnimatorControllers();

        // Decrease timers
        slidingTimerCounter = slidingTimerCounter - Time.deltaTime;
        slidingCooldownCounter = slidingCooldownCounter - Time.deltaTime;

        // Exit early if player is dead
        if (isDead)
        {
            return;
        }

        // Move player if unlocked and not hitting a wall
        if (playerUnlocked && !wallDetected)
        {
            MovementSetup();
        }

        // Reset double jump when landing on ground
        if (isGrounded)
        {
            DoubleJumpAvailable = true;
        }
        // Increase speed at milestones
        SpeedController();
        // End slide if conditions are met
        CheckForSlidingCancel();
        // Process player input
        CheckInput();
    }

    // Handle player death sequence
    public IEnumerator Death()
    {
        // Play death sound effect
        AudioManager.instance.PlaySFX(2);
        isDead = true;
        // Apply knockback force
        rb.velocity = knockbackDirection;
        // Trigger death animation
        anim.SetBool("isDead", true);

        // Time to wait for death animation to finish
        yield return new WaitForSeconds(0.5f);

        // Stops all movement after death animation
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // Stops forces and collisions moving it
        rb.simulated = false; // Fully disable physics simulation

        // Restart level after a short delay
        yield return new WaitForSeconds(3f);
        GameManager.instance.RestartLevel();
    }

    // Cancel death state
    public void cancelDeath()
    {
        isDead = false;
    }

    // Set player velocity based on current state
    private void MovementSetup()
    {
        // Stop movement if hitting a wall
        if (wallDetected)
        {
            ResetSpeed();
            return;
        }
        // Use sliding speed during slide
        if (isSliding)
        {
            rb.velocity = new Vector2(slidingSpeed, rb.velocity.y);
        }
        else
        {
            // Keeps y velocity the same, only changes x velocity when moving
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
    }
    #region SpeedControls
    // Reset speed to default values
    private void ResetSpeed()
    {
        moveSpeed = defaultMoveSpeed;
        milestoneIncreaser = defaultMilestoneIncreaser;
    }

    // Increase player speed at distance milestones
    private void SpeedController()
    {
        // Exit if already at max speed
        if (moveSpeed == maxSpeed)
        {
            return;
        }

        // Check if player passed the speed milestone
        if (transform.position.x > speedMilestone)
        {
            // Set next milestone
            speedMilestone += milestoneIncreaser;

            // Increase speed and milestone distance
            moveSpeed *= speedMult;
            milestoneIncreaser *= speedMult;

            // Cap speed at maximum
            if (moveSpeed > maxSpeed)
            {
                moveSpeed = maxSpeed;
            }
        }
    }
    #endregion


    // End slide if timer expired and no ceiling above
    private void CheckForSlidingCancel()
    {
        if (slidingTimerCounter < 0 && !ceilingDetection)
        {
            isSliding = false;
        }

    }

    #region Inputs
    // Activate sliding if cooldown finished
    private void SlidingButton()
    {
        if (slidingCooldownCounter < 0)
        {
            isSliding = true;
            slidingTimerCounter = slidingTime;
            slidingCooldownCounter = slidingCooldown;
        }
    }

    // Handle jump and double jump logic
    private void JumpButton()
    {
        // Does not allow jumping when sliding
        if (isSliding)
        {
            return;
        }

        // Regular jump when on ground
        if (isGrounded)
        {
            // Keeps x velocity the same, only changes y velocity when jumping
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            AudioManager.instance.PlaySFX(0);
        }
        // Double jump when in air
        else if (DoubleJumpAvailable)
        {
            DoubleJumpAvailable = false;
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpPower);
            AudioManager.instance.PlaySFX(1);
        }
    }
    // Check for player input and trigger appropriate actions
    private void CheckInput()
    {
        // Jump with space bar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
        }

        // Slide with left control when grounded and not already sliding
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isSliding && isGrounded)
        {
            SlidingButton();
        }
    }
    #endregion

    #region Animations
    // Update animator parameters based on player state
    private void AnimatorControllers()
    {
        anim.SetBool("doubleJumpAvailable", DoubleJumpAvailable);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isSliding", isSliding);
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isDead", isDead);
    }

    #endregion
    // Check for collisions with ground, ceiling, and walls using raycasts
    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        ceilingDetection = Physics2D.Raycast(transform.position, Vector2.up, ceilingCheckDistance, whatIsGround);
        wallDetected = Physics2D.BoxCast(wallDetection.position, wallDetectionSize, 0, Vector2.zero, 0, whatIsGround);
    }

    // Draw collision detection gizmos in the editor
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceilingCheckDistance));
        Gizmos.DrawWireCube(wallDetection.position, wallDetectionSize);
    }
}