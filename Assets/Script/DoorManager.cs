using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DoorManager : MonoBehaviour
{
    [SerializeField] UnityEvent OnFinishDoor;


    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    { 
        StartCoroutine(TimeDoorOpening_CO());
    }

    private IEnumerator TimeDoorOpening_CO()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("IsOpen", true);
    }

    public void EndOpenDoor()
    {
        StartCoroutine(ReturnToPlayerCamera_CO());
    }
    
    private IEnumerator ReturnToPlayerCamera_CO()
    {
        yield return new WaitForSeconds(1f);
        OnFinishDoor.Invoke();
    }
}
