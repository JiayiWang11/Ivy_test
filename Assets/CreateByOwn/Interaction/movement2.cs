using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement2 : MonoBehaviour
{
    public float turnSpeed = 5f;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float raycastDistance = 0.5f;
    private float upperBodyHeight = 0.5f;
    Animator animator;
    Rigidbody rb;

    // 移动的矢量
    Vector3 movement;
    // 旋转角度
    Quaternion quaternion = Quaternion.identity;
    // 移动速度
    float currentSpeed;
    bool isGrounded;
     [Header("相机变换")] public Transform cameraTF;

    public FixedObjectInteraction objectInteraction;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // 获取水平和垂直方向的输入
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 检查玩家是否按下加速键（例如左Shift键）
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        animator.SetBool("Run", isRunning);
        bool isJumping = Input.GetKeyDown(KeyCode.Space);
        // 检查是否在地面上并跳跃
        if(isGrounded && isJumping)
        {
            animator.SetTrigger("Jump");
            rb.AddForce(Vector3.up * Mathf.Sqrt(1 * 0.7f * Physics.gravity.magnitude), ForceMode.VelocityChange);
        }
        // 检查是否按下 C 键来控制蹲下动作
        bool isCrouching = Input.GetKey(KeyCode.C);
        animator.SetBool("Crouch", isCrouching);
        

        // 根据是否按下Shift键来决定移动速度
        currentSpeed = isRunning ? runSpeed : walkSpeed;
        if (isCrouching)
        {
            currentSpeed = walkSpeed / 2;
        }

        // 设置人物移动的方向
        movement.Set(horizontal, 0f, vertical);
        movement.Normalize();
        Vector3 moveDirection = cameraTF.TransformDirection(movement);
/*
        // 将输入的方向矢量转换为相对于角色当前朝向的世界空间方向
        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
        moveDirection.Normalize();
*/
        // 定义游戏人物是否移动的bool值
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool iswalking = hasHorizontalInput || hasVerticalInput;
        animator.SetBool("Walk", iswalking);
        animator.SetBool("Idle", !iswalking);
        // 旋转的过渡
        // 旋转角色的方向
         if (moveDirection.magnitude >= 0.1f)
                RotatePlayer(moveDirection);
        if (iswalking)
        {
            bool obstacleAtUpperBody = Physics.Raycast(transform.position + Vector3.up * upperBodyHeight, moveDirection, raycastDistance);
            if (!obstacleAtUpperBody)
            {
                // Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                // rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime));
                rb.MovePosition(rb.position + moveDirection * currentSpeed * Time.deltaTime);
            }
        } else
        {
            // 保持原地不动
            rb.MovePosition(rb.position);
            animator.SetBool("Idle", true);
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
        }

        // 调用 FixedObjectInteraction 脚本的方法来处理门和抽屉的交互
        objectInteraction.ToggleDoor();
        objectInteraction.ToggleDrawer();
        
    }
     void RotatePlayer(Vector3 moveDirection)
    {
        // 通过移动方向旋转玩家
        Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        // 将旋转限制为只影响 Y 轴
        toRotation = Quaternion.Euler(0f, toRotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 15f);
    }
    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }



}