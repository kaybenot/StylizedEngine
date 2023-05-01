using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : IInputManager
{
    public Action<Vector2> OnMoveInput
    {
        get => inputBlocked ? null : onMoveInput;
        set => onMoveInput = value;
    }
    public Action<float> OnLookInput
    {
        get => inputBlocked ? null : onLookInput;
        set => onLookInput = value;
    }
    public Action<bool> OnRun
    {
        get => inputBlocked ? null : onRun;
        set => onRun = value;
    }

    /// <summary>
    /// Zeroes inputs and blocks new ones
    /// </summary>
    public bool InputBlocked
    {
        get => inputBlocked;
        set
        {
            if (value)
            {
                onMoveInput?.Invoke(Vector2.zero);
                onLookInput?.Invoke(0f);
                onRun?.Invoke(false);
            }

            inputBlocked = value;
        }
    }

    private bool inputBlocked = false;
    private Action<Vector2> onMoveInput;
    private Action<float> onLookInput;
    private Action<bool> onRun;
    
    public void Reset()
    {
        onMoveInput = null;
        onLookInput = null;
        onRun = null;
        inputBlocked = false;
    }
}
