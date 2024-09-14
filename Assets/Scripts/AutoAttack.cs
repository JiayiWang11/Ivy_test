using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    public Transform player;  // 玩家的位置
    private Animator animator; // 敌人的动画控制器
    public float attackDistance = 1.0f;  // 攻击距离
    public float detectionRadius = 3f; // 检测范围
    public float viewAngle = 120f; // 敌人的可见角度
    private bool isPlayerInSight = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        DetectPlayerInView();

        float distance = Vector3.Distance(transform.position, player.position);

        if (isPlayerInSight && distance <= attackDistance)
        {
            animator.SetBool("IsAttack", true);
        }
        else
        {
            animator.SetBool("IsAttack", false);
        }
    }

    void DetectPlayerInView()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer <= detectionRadius)
        {
            Vector3 forward = transform.forward;
            float angleToPlayer = Vector3.Angle(forward, directionToPlayer);

            if (angleToPlayer <= viewAngle / 2f) // 检查玩家是否在视角范围内
            {
                isPlayerInSight = true;
                return;
            }
        }
        isPlayerInSight = false;
    }

}
