using UnityEngine;

public class LeverGame : MonoBehaviour
{
    [SerializeField] private PlayerInputs inputs;
    [SerializeField] Animator puzzleDoorAnimator;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("In zone");
            if (inputs.InteractPressed)
            {
                Debug.Log("Interact pressed in zone");
                puzzleDoorAnimator.SetBool("IsOpen", true);
            }
        }
    }
}
