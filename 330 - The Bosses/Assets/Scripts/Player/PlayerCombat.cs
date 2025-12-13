using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int attackDamage = 10;
    public float attackRange = 0.6f; // change when needed / based on weapons
    public float attackCooldown = 0.3f;
    public LayerMask enemyLayer;

    private bool isAttacking = false;

    public void TryAttack()
    {
        if (!isAttacking)
        {
            StartCoroutine(MeleeAttackRoutine());
        }
    }

    // --- Melee Attack Functionality ---
    System.Collections.IEnumerator MeleeAttackRoutine()
    {
        isAttacking = true;

        // TODO: Enable the sword's hitbox here
        // MeleeHitbox.gameObject.SetActive(true);

        // Wait for the duration of the attack animation
        // 0.3 seconds is a placeholder. Get the real duration from the animation clip!
        yield return new WaitForSeconds(0.3f);

        // Hit detection
        Vector2 pos = transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(pos, attackRange, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            Health h = hit.GetComponent<Health>();
            if (h != null)
            {
                h.TakeDamage(attackDamage);
            }
        }

        // Attack cooldown
        yield return new WaitForSeconds(attackCooldown);

        // Allow movement and subsequent attacks again
        isAttacking = false;
    }

}
