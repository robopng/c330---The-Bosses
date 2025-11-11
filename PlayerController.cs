using UnityEngine;
using System.Collections; // Needed for IEnumerator

// Good practice to ensure the Input script is also on this GameObject
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // --- Public variables exposed in the Unity Inspector ---
    public float moveSpeed = 5f;
    public Animator animator; // Link this in the Inspector
    public float meleeAttackDuration = 0.3f; // Set this to match your animation

    // --- Private components ---
    private Rigidbody2D rb;
    private PlayerInput input; // <-- Reference to our new input script
    private Vector2 movement;
    private Vector2 lastMovement; // <-- Added to remember idle direction
    private bool isAttacking = false; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>(); // <-- Get the reference
        lastMovement = new Vector2(0, -1); // Default to facing down
    }

    // Called every frame to check for input
    void Update()
    {
        // 1. Get WASD Input (Movement)
        // We now read from the input script, not from Unity's Input manager
        movement = input.MovementInput;

        // 2. Get Left Click Input (Attack)
        // We read the "AttackPressed" flag from the input script
        if (input.AttackPressed && !isAttacking)
        {
            isAttacking = true;
            // Tell the Animator to play the Attack animation
            animator.SetTrigger("Attack"); 
            
            // Start the attack routine (see separate function below)
            StartCoroutine(MeleeAttackRoutine()); 
        }

        // 3. Control Animations based on Input
        // Store the last non-zero movement direction
        if (movement.sqrMagnitude > 0.01f)
        {
            lastMovement = movement;
        }

        // Set all animator parameters
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetFloat("lastHorizontal", lastMovement.x);
        animator.SetFloat("lastVertical", lastMovement.y);

        // TODO: Add animation logic for Dash and Ranged Attack
        // Example:
        // if (input.DashPressed) animator.SetTrigger("Dash");
        // animator.SetBool("IsCharging", input.IsCharging);
    }

    // Called at fixed intervals for physics updates
    void FixedUpdate()
    {
        // Only allow movement if not melee attacking AND not charging a ranged attack
        if (!isAttacking && !input.IsCharging)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // --- Melee Attack Functionality ---
    System.Collections.IEnumerator MeleeAttackRoutine()
    {
        // TODO: Enable the sword's hitbox here
        // MeleeHitbox.gameObject.SetActive(true);

        // Wait for the duration of the attack animation
        yield return new WaitForSeconds(meleeAttackDuration); 

        // TODO: Disable the sword's hitbox after the attack is done
        // MeleeHitbox.gameObject.SetActive(false);

        // Allow movement and subsequent attacks again
        isAttacking = false;
    }
}