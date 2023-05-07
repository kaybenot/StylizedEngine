using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private TMP_Text frameTime;
    [SerializeField] private TMP_Text sessionID;
    [SerializeField] private TMP_Text sessionObjCount;
    [SerializeField] private TMP_Text sessionDataCount;
    [SerializeField] private TMP_Text gameDay;
    [SerializeField] private TMP_Text gameTime;
    [SerializeField] private TMP_Text commandListenerCount;
    [SerializeField] private TMP_Text gameState;
    [SerializeField] private TMP_Text playerPosition;
    [SerializeField] private TMP_Text cameraPosition;

    [Inject] private ISession session;
    [Inject] private ICommandProcessor processor;
    [Inject] private IPauseManager pauseManager;

    private float lastFrameTime;
    private int sessionObjectCount;
    private GameTime gameTimeComponent;
    private Player player;
    private CameraFollow cam;

    private void Awake()
    {
        // Update info once more after pause state has been changed
        pauseManager.OnPauseChanged += (paused) => UpdateDebugInfo();
    }

    private void OnEnable()
    {
        sessionObjectCount = session.GetAllSessionObjects().Count();
        gameTimeComponent = FindObjectOfType<GameTime>();
        player = FindObjectOfType<Player>();
        cam = FindObjectOfType<CameraFollow>();
    }

    private void Update()
    {
        lastFrameTime = Time.unscaledDeltaTime;
    }

    private void FixedUpdate()
    {
        UpdateDebugInfo();
    }

    private void UpdateDebugInfo()
    {
        var fps = (int)(1f / lastFrameTime);
        frameTime.text = fps switch
        {
            >= 60 => $"<color=\"green\">{fps}FPS</color> ({lastFrameTime * 1000f:N2}ms)",
            >= 30 => $"<color=\"yellow\">{fps}FPS</color> ({lastFrameTime * 1000f:N2}ms)",
            _ => $"<color=\"red\">{fps}FPS</color> ({lastFrameTime * 1000f:N2}ms)"
        };
        
        sessionID.text = $"Session ID: {(session.Initialized ? session.ID : "No session")}";
        sessionObjCount.text = $"Session object count: {sessionObjectCount}";
        sessionDataCount.text = $"Session data count: {session.GetDataCount()}";

        var currTime = gameTimeComponent.GetTime();
        gameTime.text = $"Game time: {currTime.hour}:{currTime.minute}:{currTime.second}";

        commandListenerCount.text = $"Command listener count: {processor.GetListenerCount()}";
        gameState.text = $"State: {(pauseManager.Paused ? "Paused" : "Running")}";
        playerPosition.text = $"Player position: {(player != null ? player.transform.position : "Player not found!")}";
        cameraPosition.text = $"Camera position: {(cam != null ? cam.transform.position : "Camera not found!")}";
    }
}
