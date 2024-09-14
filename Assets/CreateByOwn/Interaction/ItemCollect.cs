using UnityEngine;

public class ItemCollect : MonoBehaviour
{
    public string itemName; // 物品的名称

    private void OnTriggerEnter(Collider other)
    {
        // 检查玩家是否进入触发器
        if (other.CompareTag("Player"))
        {
            Debug.Log("You can pick up " + itemName + " by pressing Q.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // 检查玩家是否按下 Q 键
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.Q))
        {
            PlayerPack pack = other.GetComponent<PlayerPack>();
            if (pack != null)
            {
                if (!pack.IsFull())
                {
                    // 将物品添加到玩家的背包
                    pack.AddItem(itemName);
                    Destroy(gameObject); // 拾取后销毁物品
                }
                else
                {
                    Debug.Log("Cannot pick up " + itemName + ". pack is full.");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 当玩家离开触发器区域
        if (other.CompareTag("Player"))
        {
            Debug.Log("You left the area. Cannot pick up " + itemName + ".");
        }
    }
}
