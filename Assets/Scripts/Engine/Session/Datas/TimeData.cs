using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeData : ObjectData
{
    public (int hour, int minute, int second) CurrentTime = (11, 0, 0);
    public int Day;
    public float SpeedMultiplier = 60f;
}
