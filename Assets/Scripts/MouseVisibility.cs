using UnityEngine;
using UnityEngine.UI;

public class MouseVisibility : MonoBehaviour
{
    private bool isCursorVisible = false;
    private bool isCursorLocked = true;

    void Start()
    {
        Cursor.visible = isCursorVisible;
        Cursor.lockState = isCursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorVisibility();
        }
    }

    public void ToggleCursorVisibility()
    {
        isCursorVisible = !Cursor.visible; // Update isCursorVisible based on the actual cursor visibility
        Cursor.visible = isCursorVisible;
        Cursor.lockState = isCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
    public void Exit()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
