using System.Collections.Generic;
using UnityEngine;

public class PlayerPack : MonoBehaviour
{
    public int capacity = 6; // 背包容量
    public List<string> items; // 用来存储物品的列表

    void Start()
    {
        items = new List<string>(capacity); // 初始化背包
    }

    // 添加物品到背包
    public bool AddItem(string item)
    {
        if (items.Count < capacity)
        {
            items.Add(item);
            Debug.Log("Added " + item + " to pack.");
            return true;
        }
        else
        {
            Debug.Log("pack is full!");
            return false;
        }
    }

    // 检查背包是否已满
    public bool IsFull()
    {
        return items.Count >= capacity;
    }

    // 显示背包中的所有物品
    public void ShowPack()
    {
        Debug.Log("Pack contains:");
        foreach (var item in items)
        {
            Debug.Log(item);
        }
    }
}
