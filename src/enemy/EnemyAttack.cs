using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackCooldown = 2f;
    public int attackDamage = 10;
    private float lastAttackTime = 0f;
    private Animator animator;
    public Transform player;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // 玩家离开攻击范围 -> 回到追击状态
        if (distance > 1.6f)
        {
            GetComponent<EnemyChase>().enabled = true;
            enabled = false;
            return;
        }

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    void Attack()
    {
        if (animator != null)
            animator.SetTrigger("Attack");

        Debug.Log("Enemy attacks player for " + attackDamage + " damage!");
    }
}
