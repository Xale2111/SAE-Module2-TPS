using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class LeverGame : MonoBehaviour
{
    [SerializeField] private PlayerInputs inputs;
    [SerializeField] private DoorManager doorManager;
    [SerializeField] CinemachineCamera doorCamera;
    [SerializeField] CinemachineCamera playerCamera;

    [SerializeField] UnityEvent OnDoorOpenBegin;
    [SerializeField] UnityEvent OnDoorOpenFinish;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("In zone");
            if (inputs.InteractPressed)
            {
                Debug.Log("Interact pressed in zone");
                OpenDoorBegin();
            }
        }
    }

    private void OpenDoorBegin()
    {
        doorCamera.Priority = playerCamera.Priority+1;   
        OnDoorOpenBegin.Invoke();        
    }

    public void OpenDoorFinish()
    {
        doorCamera.Priority = 0;
        OnDoorOpenFinish.Invoke();
    }
}
