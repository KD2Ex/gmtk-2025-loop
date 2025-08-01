using System;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D loopBoundingShape;
    [SerializeField] private PolygonCollider2D hubBoundingShape;
    [SerializeField] private float hubOrthoSize = 16f;
    [SerializeField] private bool startInHub = true;
    
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineConfiner2D confier;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        confier = GetComponent<CinemachineConfiner2D>();
    }

    private void Start()
    {
        if (startInHub)
            SetHubBounds();
        else
            SetLoopBounds();
    }

    public void SetHubBounds()
    {
        confier.m_BoundingShape2D = hubBoundingShape;
        confier.m_MaxWindowSize = 0;
        confier.m_Damping = 0;
        virtualCamera.m_Lens.OrthographicSize = hubOrthoSize;
    }

    public void SetLoopBounds()
    {
        confier.m_BoundingShape2D = loopBoundingShape;
        confier.m_MaxWindowSize = 10.4f;
        confier.m_Damping = .32f;
        virtualCamera.m_Lens.OrthographicSize = 19;
    }
}
