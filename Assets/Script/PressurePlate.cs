using UnityEngine;
using UnityEngine.Events;



public class PressurePlate : MonoBehaviour
{
    [SerializeField] private UnityEvent OnActivate;
    [SerializeField] private PressurePlateColor color;

    public bool state = false;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {            
            if (!state)
            {
                state = true;
                OnActivate.Invoke();
            }
        }
    }


}
