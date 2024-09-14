using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class NPCPatrol : MonoBehaviour
{
    public float patrolSpeed = 2f; // 巡逻时的速度
    public List<GameObject> roomBounds; // NPC所在房间的多个BoxCollider
    private NavMeshAgent agent;
    private List<BoxCollider> roomColliders; // 缓存房间的BoxCollider列表

    private const float minPatrolDistance = 2f; // 巡逻点的最小距离

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;

        // 缓存房间的BoxCollider
        roomColliders = new List<BoxCollider>();
        foreach (var room in roomBounds)
        {
            BoxCollider collider = room.GetComponent<BoxCollider>();
            if (collider != null)
            {
                roomColliders.Add(collider);
            }
        }

        // 设置第一个巡逻目标
        SetNewPatrolTarget();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            SetNewPatrolTarget();
        }

        // 检测前方是否有障碍物
        if (IsObstacleAhead())
        {
            SetNewPatrolTarget();
        }
    }

    // 设置新的巡逻目标点
    void SetNewPatrolTarget()
    {
        if (roomColliders.Count == 0) return;

        Vector3 randomPoint;
        bool validPointFound = false;

        while (!validPointFound)
        {
            randomPoint = GetRandomPointInRoom();

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                randomPoint = hit.position;

                if (Vector3.Distance(transform.position, randomPoint) > minPatrolDistance)
                {
                    validPointFound = true;
                    agent.destination = randomPoint;
                }
            }
        }
    }

    // 在房间内生成一个随机点 (只考虑X和Z坐标)
    Vector3 GetRandomPointInRoom()
    {
        BoxCollider selectedCollider = roomColliders[Random.Range(0, roomColliders.Count)];

        Vector3 minBounds = selectedCollider.bounds.min;
        Vector3 maxBounds = selectedCollider.bounds.max;

        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomZ = Random.Range(minBounds.z, maxBounds.z);

        // 使用当前对象的Y坐标，确保NPC保持在相同的高度
        return new Vector3(randomX, transform.position.y, randomZ);
    }

    // 检测前方是否有障碍物
    private bool IsObstacleAhead()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1.0f))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                return true;
            }
        }
        return false;
    }

    // 检测NPC是否撞上物品，改变方向
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // 如果碰到障碍物，重新设置一个新的巡逻目标点
            SetNewPatrolTarget();
        }
    }
}
