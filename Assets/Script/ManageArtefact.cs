using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;

public class ManageArtefact : MonoBehaviour
{
    [SerializeField] private Transform playerSetupPosition;
    [SerializeField] private PlayerController player;
    
    [SerializeField] private float size = 3;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Animator UI_Animator;
    
    [SerializeField] private SplineAnimate cameraSpline;
    [SerializeField] private CinemachineCamera endCamera;
    
    [SerializeField] private GameObject artefact;
    
    bool _inCinematic = false;

    private CinemachineSplineDolly _camDolly;
    
    private AudioSource _audioSource;
    
    private void Start()
    {
        canvas.SetActive(false);
        _camDolly = endCamera.GetComponent<CinemachineSplineDolly>();
        _audioSource= GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!_inCinematic)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, Vector3.one * size);

            if (colliders.Length > 0)
            {
                foreach (Collider colliding in colliders)
                {
                    if (colliding.gameObject.CompareTag("Player"))
                    {
                        _inCinematic = true;
                        player.SetCanMove(false);
                        canvas.SetActive(true);

                        StartCoroutine(EndCinematic_CO());
                        //Start end cinematic
                    }
                }
            }
        }

        if (_camDolly.CameraPosition >= 1)
        {   
            StartCoroutine(GoToCredits_CO());
        }
    }

    private IEnumerator EndCinematic_CO()
    {
        UI_Animator.SetTrigger("PickedUpArtefact");
        yield return new WaitForSeconds(1.2f);
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1f);
        player.transform.position = playerSetupPosition.position;
        player.transform.rotation = playerSetupPosition.rotation;
        endCamera.Priority = 10;
        yield return new WaitForSeconds(1f);
        player.PickUpArtefact();
        cameraSpline.Play();
        endCamera.GetComponent<CinemachineSplineDolly>().AutomaticDolly.Enabled = true;
    }

    private IEnumerator GoToCredits_CO()
    {
        yield return new WaitForSeconds(4.2f);
        SceneManager.LoadScene("Credits");
    }

    public void DestroyArtefact()
    {
        Destroy(artefact);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(255,255,0,0.5f);
        Gizmos.DrawCube(transform.position , Vector3.one * size*2);
    }
}
