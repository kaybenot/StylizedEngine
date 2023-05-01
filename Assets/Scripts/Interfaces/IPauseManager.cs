using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPauseManager
{
    Action<bool> OnPauseChanged { get; set; }

    void Pause();
    void Unpause();
}
