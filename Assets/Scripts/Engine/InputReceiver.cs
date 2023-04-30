using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(PlayerInput))]
public class InputReceiver : MonoBehaviour
{
    [Inject] private IInputManager inputManager;

    private Vector3 input;

    public void Move(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            inputManager.OnMoveInput?.Invoke(Vector2.zero);
            return;
        }
        
        if (!context.performed)
            return;

        inputManager.OnMoveInput?.Invoke(context.ReadValue<Vector2>());
    }
    
    public void Run(InputAction.CallbackContext context)
    {
        if (context.started)
            inputManager.OnRun?.Invoke(true);
        if (context.canceled)
            inputManager.OnRun?.Invoke(false);
    }
    
    public void Look(InputAction.CallbackContext context)
    {
        if (context.performed)
            inputManager.OnLookInput?.Invoke(context.ReadValue<float>());
        if (context.canceled)
            inputManager.OnLookInput?.Invoke(0f);
    }
}
