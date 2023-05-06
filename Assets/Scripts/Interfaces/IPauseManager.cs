using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPauseManager
{
    Action<bool> OnPauseChanged { get; set; }
    bool Paused { get; }

    void Pause();
    void Unpause();
}
