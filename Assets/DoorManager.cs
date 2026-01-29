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
        animator.SetBool("IsOpen", true);
    }

    public void BeginOpenDoor()
    {
    }

    public void EndOpenDoor()
    {
        OnFinishDoor.Invoke();
    }
}
