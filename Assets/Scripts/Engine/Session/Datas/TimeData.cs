using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeData : ObjectData
{
    public TimeData(GameObject prefab) : base(prefab)
    {
        CurrentTime = (11, 0, 0);
        SpeedMultiplier = 60f;
    }

    public (int hour, int minute, int second) CurrentTime;
    public int Day;
    public float SpeedMultiplier;
}
