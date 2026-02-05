using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class SecretWallManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera wallCamera;
    [SerializeField] private CinemachineCamera playerCamera;
    
    
    [SerializeField] private UnityEvent onWallOpenEnd;
    private Animator _animator;
    
    private void Start()
    {
        wallCamera.Priority = 0;   
        _animator = GetComponent<Animator>();
    }
    
    public void OpenSecretWall()
    {
        StartCoroutine(OpenSecretWall_CO());
    }
    
    public void OnEndOpenSecretWall()
    {
        StartCoroutine(ReturnToPlayerCamera_CO());
    }

    private IEnumerator OpenSecretWall_CO()
    {
        yield return new WaitForSeconds(1f);
        wallCamera.Priority = playerCamera.Priority+1;
        Debug.Log("OpenSecretWall");
        yield return new WaitForSeconds(1f);
        _animator.SetTrigger("OpenSecretWall");
        
    }

    private IEnumerator ReturnToPlayerCamera_CO()
    {
        yield return new WaitForSeconds(1f);
        wallCamera.Priority = 0;
        onWallOpenEnd.Invoke();
    }

}
