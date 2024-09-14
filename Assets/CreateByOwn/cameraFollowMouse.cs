using UnityEngine;

public class CameraFollowMouse : MonoBehaviour
{
    [SerializeField] private Transform playerBody;  // 角色的Transform
    [SerializeField] private float mouseSensitivity = 100f;  // 鼠标灵敏度
    private float xRotation = 0f;  // 摄像机的垂直旋转角度
    private float yRotation = 0f;  // 水平旋转角度
    [SerializeField] private Vector3 offset = new Vector3(0, 1.7f, -2f);  // 相机相对于角色的偏移量
    [SerializeField] private float smoothSpeed = 0.125f;  // 平滑跟随速度

    void Start()
    {
        // 隐藏并锁定鼠标光标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;  // 隐藏光标
    }
        [Header("玩家的Transform组件")] public Transform target;
    public float rotationSpeed = 5f;
    public float followSpeed = 5f;

    [Header("摄像机到目标的距离")] public float distance = 5f;
    [Header("最小距离")] public float minDistance = 2f;
    [Header("最大距离")] public float maxDistance = 10f;
    [Header("缩放速度")] public float zoomSpeed = 2f;
		[Header("最小垂直角度")] public float minVerticalAngle = -30f;
    [Header("最大垂直角度")] public float maxVerticalAngle = 60f;
    private float currentVerticalAngle = 0f;
    void Update()
    {
        float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed;
        transform.RotateAround(target.position, Vector3.up, horizontalRotation);
        float verticalRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
        //transform.RotateAround(target.position, transform.right, -verticalRotation);
        float newVerticalAngle = currentVerticalAngle - verticalRotation;

        // 限制垂直角度
        newVerticalAngle = Mathf.Clamp(newVerticalAngle, minVerticalAngle, maxVerticalAngle);

        // 计算旋转的变化量
        float angleDelta = newVerticalAngle - currentVerticalAngle;
        currentVerticalAngle = newVerticalAngle;

        // 进行垂直旋转
        transform.RotateAround(target.position, transform.right, angleDelta);
        // 使摄像机朝向目标
        //transform.LookAt(target.position);

        // 使用滚轮缩放摄像机距离
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // 调整摄像机位置
        Vector3 offset = -transform.forward * distance;
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * followSpeed);
         // 使角色随鼠标左右旋转
        playerBody.Rotate(Vector3.up * horizontalRotation);
    
    }


/*
    void Update()
    {

        // 获取鼠标移动量
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 控制垂直旋转（Pitch）
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // 限制垂直旋转的角度范围

        // 控制水平旋转（Yaw），这里不对水平旋转做任何限制
        yRotation += mouseX;

        // 设置摄像机的旋转，xRotation 控制上下，yRotation 控制左右
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // 使角色随鼠标左右旋转
        playerBody.Rotate(Vector3.up * mouseX);
    }
    void LateUpdate()
    {
        // 计算相机相对于角色的位置
        Vector3 desiredPosition = playerBody.position + playerBody.rotation * offset;

        // 平滑跟随角色的位置
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    
        // 相机看向角色的前方
        transform.LookAt(playerBody.position + Vector3.up * 1.5f);
    }*/
}