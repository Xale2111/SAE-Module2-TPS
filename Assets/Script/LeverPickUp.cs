using UnityEngine;
using UnityEngine.Events;

public class LeverPickUp : MonoBehaviour
{
    [SerializeField] private UnityEvent OnPickUp;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Pick up lever");
            OnPickUp.Invoke();
            Destroy(gameObject);
        }
    }
}
