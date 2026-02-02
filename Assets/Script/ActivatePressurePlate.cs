using UnityEngine;
using UnityEngine.Events;

enum PressurePlateColor
{
    Red, Green, Blue, Yellow
}

public class ActivatePressurePlate : MonoBehaviour
{
    [SerializeField] private UnityEvent OnActivate;
    [SerializeField] private PressurePlateColor color; 
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnActivate.Invoke();
        }
        
    }
}
