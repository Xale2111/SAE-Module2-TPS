using System;
using Unity.Cinemachine;
using UnityEngine;

public class StairsCamera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera stairsCamera;
    [SerializeField] private CinemachineCamera playerCamera;

    private void Start()
    {
        stairsCamera.Priority = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) stairsCamera.Priority = playerCamera.Priority+1;
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) stairsCamera.Priority = 0;
    }
}
