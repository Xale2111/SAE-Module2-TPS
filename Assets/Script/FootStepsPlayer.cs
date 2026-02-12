using UnityEngine;

public class FootStepsPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip[] footstepClip;
    
    public void PlayFootStep()
    {
        audioSource.PlayOneShot(footstepClip[Random.Range(0, footstepClip.Length)]);
    }
}
