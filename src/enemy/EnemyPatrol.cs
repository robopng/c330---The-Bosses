using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float speed = 2f;
    public float detectRange = 20f;   // 发现玩家距离
    public float chaseTriggerRange = 10f; // 追击距离
    public Transform player;

    private int currentPoint = 0;
    private bool isChasing = false;

    void Update()
    {
        if (player == null || patrolPoints.Length == 0) return;

        // 计算2D平面距离
        float distanceToPlayer = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.y),
            new Vector2(player.position.x, player.position.y)
        );

        // 🟢 距离大于detectRange：继续巡逻
        if (distanceToPlayer > detectRange)
        {
            if (isChasing)
            {
                // 玩家跑远 -> 回到巡逻状态
                isChasing = false;
                GetComponent<EnemyChase>().enabled = false;
                Debug.Log("🔵 Player too far — resume patrol");
            }
            Patrol();
            return;
        }

        // 🟡 玩家进入视野范围（detectRange）但未到追击距离
        if (distanceToPlayer <= detectRange && distanceToPlayer > chaseTriggerRange)
        {
            // 可加一个“面向玩家”的逻辑
            Vector2 direction = (player.position - transform.position).normalized;
            transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
            Debug.Log("🟡 Player detected but not close enough — watching");
            Patrol(); // 仍然巡逻
            return;
        }

        // 🔴 玩家进入追击范围
        if (distanceToPlayer <= chaseTriggerRange && !isChasing)
        {
            isChasing = true;
            Debug.Log($"🔴 Start chasing! Distance={distanceToPlayer:F2}");
            GetComponent<EnemyChase>().enabled = true;
            enabled = false; // 暂停巡逻逻辑
        }
    }

    void Patrol()
    {
        Transform target = patrolPoints[currentPoint];
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        float distToTarget = Vector2.Distance(transform.position, target.position);
        if (distToTarget < 0.1f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
        }
    }
}
