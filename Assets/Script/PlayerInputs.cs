using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public Vector2 InputMove;
    public bool InputIsRunning;
    
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
    
}
