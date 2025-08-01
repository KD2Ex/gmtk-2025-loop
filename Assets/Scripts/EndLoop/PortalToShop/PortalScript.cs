using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;


public class PortalScript : MonoBehaviour
{
    public bool activated;
    public bool blockOnExit;
    
    
    
    [SerializeField] public Transform exit;
    [SerializeField] private Transform cameraFollow;
    [SerializeField] private CinemachineVirtualCamera cinemachine;

    private CameraManager cameraManager;

    private void Awake()
    {
        cameraManager = cinemachine.GetComponent<CameraManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        exit.GetComponent<PortalScript>().activated = true;
        Vector3 oldPosition =  other.gameObject.transform.position;
        other.gameObject.transform.position = exit.transform.position;
        Vector3 displacement = exit.transform.position - oldPosition;
        cinemachine.OnTargetObjectWarped(other.gameObject.transform, displacement);

        if (!blockOnExit) // enetered loop
        {
            //cinemachine.GetComponent<CinemachineConfiner2D>().enabled = false;
            cameraManager.SetLoopBounds();
            GameManager.instance.EnterLoop();
        }
        else // entered hub
        {
            //cinemachine.GetComponent<CinemachineConfiner2D>().enabled = true;
            cameraManager.SetHubBounds();
            GameManager.instance.ExitLoop();
        }
        
        activated = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (blockOnExit) return;
        activated = false;
    }
}
