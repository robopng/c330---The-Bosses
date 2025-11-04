using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // --- Public variables exposed in the Unity Inspector ---
    public float moveSpeed = 5f;
    public Animator animator; // Link this in the Inspector to the character's Animator component.

    // --- Private components ---
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isAttacking = false; // Prevents movement/other actions while attacking.

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Called every frame to check for input
    void Update()
    {
        // 1. Get WASD Input (Movement)
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right Arrow
        movement.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down Arrow
        movement.Normalize(); // Prevents diagonal movement from being too fast.

        // 2. Get Left Click Input (Attack)
        if (Input.GetMouseButtonDown(0) && !isAttacking) // 0 is the Left Mouse Button
        {
            isAttacking = true;
            // Tell the Animator to play the Attack animation
            animator.SetTrigger("Attack"); 
            
            // Start the attack routine (see separate function below)
            StartCoroutine(MeleeAttackRoutine()); 
        }

        // 3. Control Animations based on Input
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude); // Check if we are moving at all.
    }

    // Called at fixed intervals for physics updates
    void FixedUpdate()
    {
        // Only allow movement if the character is not currently attacking
        if (!isAttacking)
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
        // 0.3 seconds is a placeholder. Get the real duration from the animation clip!
        yield return new WaitForSeconds(0.3f); 

        // TODO: Disable the sword's hitbox after the attack is done
        // MeleeHitbox.gameObject.SetActive(false);

        // Allow movement and subsequent attacks again
        isAttacking = false;
    }
}