using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedObjectInteraction : MonoBehaviour
{
    private doorController dc; // 门的控制器
    private DrawerController drawerController; // 抽屉的控制器

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            dc = other.GetComponent<doorController>();
        }
        else if (other.CompareTag("Drawer"))
        {
            drawerController = other.GetComponent<DrawerController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            dc = null;
        }
        else if (other.CompareTag("Drawer"))
        {
            drawerController = null;
        }
    }

    public void ToggleDoor()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (dc != null)
            {
                dc.ToggleDoor();
            }
        }
    }

    public void ToggleDrawer()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (drawerController != null)
            {
                drawerController.ToggleDrawer();
            }
        }
    }
}
