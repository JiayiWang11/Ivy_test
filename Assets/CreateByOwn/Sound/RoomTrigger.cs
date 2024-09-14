using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RoomTrigger : MonoBehaviour
{
    public AudioClip newMusicClip;  // 要播放的音乐剪辑
    public AudioClip backgroundMusicClip;  // 背景音乐剪辑
    private AudioSource audioSource;
    private BoxCollider boxCollider;
    private GameObject player;
    private bool isInTriggerArea = false;

    void Start()
    {
        // 获取 BoxCollider 组件
        boxCollider = GetComponent<BoxCollider>();

        // 获取或添加 AudioSource 组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource is missing on " + gameObject.name + ". Adding one now.");
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 获取玩家对象
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object with tag 'Player' not found in the scene.");
        }

        // 默认播放背景音乐
        audioSource.clip = backgroundMusicClip;
        audioSource.Play();
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;

            // 检查玩家的X和Z坐标是否在BoxCollider的范围内
            if (IsPlayerInBoxXZ(playerPosition))
            {
                if (!isInTriggerArea)
                {
                    audioSource.clip = newMusicClip;
                    audioSource.Play();
                    isInTriggerArea = true;
                }
            }
            else
            {
                if (isInTriggerArea)
                {
                    audioSource.clip = backgroundMusicClip;
                    audioSource.Play();
                    isInTriggerArea = false;
                }
            }
        }
    }

    private bool IsPlayerInBoxXZ(Vector3 playerPosition)
    {
        Vector3 minBounds = boxCollider.bounds.min;
        Vector3 maxBounds = boxCollider.bounds.max;

        // 检查玩家的X和Z坐标是否在BoxCollider的范围内
        return playerPosition.x >= minBounds.x && playerPosition.x <= maxBounds.x &&
               playerPosition.z >= minBounds.z && playerPosition.z <= maxBounds.z;
    }
}
