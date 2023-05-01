using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages game input.
/// UI should not be affected.
/// Reset it when creating new Game session.
/// </summary>
public interface IInputManager
{
    Action<Vector2> OnMoveInput { get; set; }
    /// <summary>
    /// Camera can rotate only in horizontal axis, so parameter is a simple float.
    /// </summary>
    Action<float> OnLookInput { get; set; }
    Action<bool> OnRun { get; set; }
    bool InputBlocked { get; set; }

    void Reset();
}
