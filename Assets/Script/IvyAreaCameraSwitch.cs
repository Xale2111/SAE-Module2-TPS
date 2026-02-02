using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class IvyAreaCameraSwitch : MonoBehaviour
{
    [SerializeField] private CinemachineCamera ivyCamera;
    [SerializeField] private CinemachineCamera playerCamera;
    
    [SerializeField] private UnityEvent OnIvyAreaEnter;
    [SerializeField] private UnityEvent OnIvyAreaExit;

    private void Start()
    {
        ivyCamera.Priority = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ivyCamera.Priority = playerCamera.Priority +1;
            OnIvyAreaEnter.Invoke();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ivyCamera.Priority = 0;
            OnIvyAreaExit.Invoke();
        }
    }
}
