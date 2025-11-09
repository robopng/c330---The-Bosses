using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 2.5f;
    public float attackRange = 1.5f;

    private Animator animator;
    private EnemyAttack attackScript;

    void Start()
    {
        animator = GetComponent<Animator>();
        attackScript = GetComponent<EnemyAttack>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // 🟢 玩家在攻击范围外 -> 继续追
        if (distance > attackRange)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            dir.z = 0;

            Vector3 newPos = transform.position + dir * chaseSpeed * Time.deltaTime;
            newPos.z = 0;
            transform.position = newPos;

        //     if (animator != null)
        //         animator.SetBool("isMoving", true);
        // }
        // else
        // {
        //     // 🔴 到达攻击范围 -> 切换到攻击逻辑
        //     if (animator != null)
        //         animator.SetBool("isMoving", false);

            attackScript.enabled = true;   // 开启攻击
            enabled = false;               // 停止追击
        }
    }
}
