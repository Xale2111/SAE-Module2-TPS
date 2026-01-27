using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public Vector2 InputMove;
    public bool InputIsRunning;
    public bool JumpIsPressed;
    public bool InteractPressed;
    
    public void OnMove(InputAction.CallbackContext context) => InputMove = context.ReadValue<Vector2>();

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            InputIsRunning = true;
        }

        if (context.canceled)
        {
            InputIsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpIsPressed = true;            
        }
    }

    public void OnInteractPressed(InputAction.CallbackContext context)
    {
        InteractPressed = context.performed;
        Debug.Log("Interact : " + InteractPressed);
    }

}
