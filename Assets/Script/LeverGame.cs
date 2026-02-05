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
    
    [SerializeField] private GameObject[] hiddenPlacedLever;
    
    [SerializeField] UnityEvent OnDoorOpenBegin;
    [SerializeField] UnityEvent OnDoorOpenFinish;
    [SerializeField] UnityEvent OnTwoLeverPicked;

    
    int pickedUpLever = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (player.GetLeverPickedUp() >0)
            {
                for (int i = 0; i < player.GetLeverPickedUp(); i++)
                {
                    hiddenPlacedLever[pickedUpLever].SetActive(true);
                    pickedUpLever++;
                }
                player.ResetLeverPickedUp();
            }

            if (pickedUpLever >= hiddenPlacedLever.Length)
            {
                OpenDoorBegin();
            }
            else if (pickedUpLever >= hiddenPlacedLever.Length-1)
            {
                OnTwoLeverPicked.Invoke();
            }

            
        }
    }

    private void OpenDoorBegin()
    {
        OnDoorOpenBegin.Invoke();        
        StartCoroutine(SwitchToDoorCamera());
        
    }

    public void OpenDoorFinish()
    {
        OnDoorOpenFinish.Invoke();
        StartCoroutine(SwitchToPlayerCamera());
    }
    
    private IEnumerator SwitchToDoorCamera()
    {
        yield return new WaitForSeconds(1f);
        doorCamera.Priority = playerCamera.Priority+1;   
    }
    
    private IEnumerator SwitchToPlayerCamera()
    {
        yield return new WaitForSeconds(1f);
        doorCamera.Priority = 0;
    }
}
