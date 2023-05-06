using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameTime : SessionObject, ICommandListener
{
    [SerializeField] private Light sun;

    [Inject] private ISession session;
    [Inject] private ICommandProcessor cmdProcessor;
    
    public string Name => "GameTime";

    private TimeData data;
    private float counter = 0f;

    private void Awake()
    {
        if (sun == null)
        {
            sun = GameObject.FindWithTag("Sun")?.GetComponent<Light>();
            if (sun == null)
            {
                Debug.LogError("There is no light with Sun tag on scene. Add this tag or drag this light in inspector");
                return;
            }
        }
    }

    public override void OnSessionInitialized()
    {
        cmdProcessor.AddListener(this);
        GetOrCreateData();
    }

    private void FixedUpdate()
    {
        if (data == null)
            return;

        // Accurate time calculation
        counter += Time.fixedDeltaTime * data.SpeedMultiplier;
        AddTime((int)counter);
        counter -= (int)counter;
    }

    public void SetTime(int hour, int minute = 0, int second = 0)
    {
        const float radPerHour = 360f / 24f;
        const float radPerMinute = 360f / (24f * 60f);
        const float radPerSecond = 360f / (24f * 60f * 60f);

        sun.transform.rotation = Quaternion.Euler(radPerHour * (hour % 24) + radPerMinute * (minute % 60) + 
                                                  radPerSecond * (second % 60) - 90f, 0f, 0f);
        data.CurrentTime = (hour, minute, second);
    }

    public void AddTime(int seconds, int minutes = 0, int hours = 0)
    {
        var s = seconds + data.CurrentTime.second;
        var m = minutes + data.CurrentTime.minute;
        var h = hours + data.CurrentTime.hour;

        if (s >= 60)
            m += s / 60;
        if (m >= 60)
            h += m / 60;
        
        SetTime(h % 24, m % 60, s % 60);
    }

    public (int hour, int minute, int second) GetTime()
    {
        return data.CurrentTime;
    }

    public string ProcessCommand(string command, string[] args)
    {
        switch (command)
        {
            case "set":
            {
                try
                {
                    switch (args.Length)
                    {
                        case 1:
                            SetTime(int.Parse(args[0]));
                            break;
                        case 2:
                            SetTime(int.Parse(args[0]), int.Parse(args[1]));
                            break;
                        case 3:
                            SetTime(int.Parse(args[0]), int.Parse(args[1]), int.Parse(args[2]));
                            break;
                        default:
                            return "Wrong argument count.";
                    }
                }
                catch (Exception e)
                {
                    return $"An error processing command! Log: {e.Message}";
                }

                return "Changed time.";
            }
            default:
                return "Invalid command.";
        }
    }

    private void GetOrCreateData()
    {
        data = session.GetData<TimeData>(ID);
        if (data != null)
            return;

        data = new TimeData();
    }
}
