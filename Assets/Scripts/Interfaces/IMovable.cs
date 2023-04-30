using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    /// <summary>
    /// Sets move direction for objects.
    /// </summary>
    /// <param name="direction">Move direction, (0,0,0) to stop movement</param>
    void Move(Vector3 direction);
}
