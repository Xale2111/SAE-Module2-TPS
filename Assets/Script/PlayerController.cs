using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputs))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f; // [m/s]
    [SerializeField] private float runSpeed = 10f; // [m/s]
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float fallSpeed = 1.7f; 
    [SerializeField] private float maxVerticalVelocity = 42f;
    [SerializeField] private float climbSpeed = 4f;
    
    [SerializeField][Range(0,1)] private float rotationSpeed = 0.05f;    
    
    [SerializeField] private GroundDetector groundDetector;
    
    [SerializeField] private UnityEvent DestroyArtefactEvent;
    
    private PlayerInputs _inputs;
    private CharacterController _characterController;
    private Animator _animator;

    private float _verticalVelocity;

    private Camera _mainCamera;
    
    bool _landingDone = true;
    
    bool _rollingDone = true;

    private bool _canMove = true;
    private bool _climbing = false;

    private float _finalRotationSpeed;

    private Transform _ivyCameraTransform;

    private Transform _spawnPoint;

    private int _pickedUpLevers = 0;
    
    private FootStepsPlayer _footStepsPlayer;
    
    private void Start()
    {
        _inputs = GetComponent<PlayerInputs>();
        _characterController = GetComponent<CharacterController>();
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _finalRotationSpeed = rotationSpeed;
        _spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        
        transform.position = _spawnPoint.position;
        
        _ivyCameraTransform = GameObject.FindGameObjectWithTag("IvyCamera").transform;
        
        _footStepsPlayer = GetComponent<FootStepsPlayer>();
    }

    private void Update()
    {
        if (_canMove)
        {

            float moveMagnitude = _inputs.InputMove.magnitude;

            Vector3 horizontalVelocity;

            if (!_climbing)
            {

                if (_landingDone && _rollingDone)
                {
                    horizontalVelocity = _inputs.IsRunning
                        ? transform.forward * (moveMagnitude * runSpeed)
                        : transform.forward * (moveMagnitude * walkSpeed);
                }
                else
                {
                    horizontalVelocity = Vector3.zero;
                }

                if (!_rollingDone)
                {
                    horizontalVelocity = transform.forward * walkSpeed / 2;
                }


                if (_verticalVelocity < maxVerticalVelocity)
                {
                    if (_characterController.velocity.y > 0)
                    {
                        _verticalVelocity += Physics.gravity.y * Time.deltaTime;
                    }
                    else
                    {
                        _verticalVelocity += Physics.gravity.y * fallSpeed * Time.deltaTime;
                    }
                }

                if (groundDetector.IsGrounded)
                {
                    if (_verticalVelocity < 0.0f)
                    {
                        _verticalVelocity = -5f;
                    }

                    if (_inputs.JumpIsPressed)
                    {                        
                        _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
                    }
                }
                else
                {
                    _inputs.JumpIsPressed = false;
                }

                Quaternion inputRotation = Quaternion.LookRotation(new Vector3(_inputs.InputMove.x, 0, _inputs.InputMove.y), Vector3.up);
                Quaternion cameraRotation = _mainCamera.transform.rotation;

                Quaternion rotation = Quaternion.Euler(0, cameraRotation.eulerAngles.y, 0) * inputRotation;

                _characterController.Move((horizontalVelocity + new Vector3(0, _verticalVelocity, 0)) * Time.deltaTime);

                if (horizontalVelocity.sqrMagnitude > 0.001f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _finalRotationSpeed);
                }



                _animator.SetFloat("AbsVelocity", moveMagnitude);
                _animator.SetBool("IsRunning", _inputs.IsRunning);
                _animator.SetBool("IsFalling", !groundDetector.IsGrounded && _characterController.velocity.y < 0.1f);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, _ivyCameraTransform.rotation, .5f);                
                if (groundDetector.IsGrounded && _inputs.InputMove.y < 0)
                {
                    //Player can back down                    
                    _characterController.Move(new Vector3((_inputs.InputMove.x*-1),0,_inputs.InputMove.y*-1) * (Time.deltaTime * walkSpeed));
                }
                else
                {
                    _characterController.Move(new Vector3((_inputs.InputMove.x*-1),_inputs.InputMove.y,0) * (Time.deltaTime * climbSpeed));
                }

                _animator.SetFloat("ClimbVelocityY", _characterController.velocity.y);
                _animator.SetFloat("ClimbVelocityX", _characterController.velocity.x);

            }
        }
    }

    private void OnLandingBegin()
    {
        _landingDone = false;
    }

    private void OnLandingFinish()
    {
        _landingDone = true;
    }

    private void OnRollBegin()
    {
        _rollingDone = false;
    }
    
    private void OnRollFinish()
    {
        _rollingDone = true;
    }

    private void PlayFootstep()
    {
        _footStepsPlayer.PlayFootStep();
    }
    
    private void SetWasRunning()
    {
        _animator.SetBool("WasRunning",_inputs.IsRunning);
    }
    
    public void SetCanMove(bool canMove)
    {
        _animator.SetBool("WasRunning", false);
        _animator.SetFloat("AbsVelocity", 0);
        Debug.Log("Can move :");
        _canMove = canMove;
    }

    public void SetRotationSpeedIvy()
    {
        _finalRotationSpeed = 1;
    }

    public void SetRotationSpeedDefault()
    {
        _finalRotationSpeed = rotationSpeed;
    }

    public void ToggleClimb()
    {
        _climbing = !_climbing;
        _animator.SetBool("IsClimbing", _climbing);
    }
    
    public void AddLeverPickedUp()
    {
        _animator.SetTrigger("LeverPickedUp");
        _pickedUpLevers++;
        SetCanMove(false);
    }
    
    public int GetLeverPickedUp()
    {
        return _pickedUpLevers;
    }
    
    public void ResetLeverPickedUp()
    {
        _pickedUpLevers = 0;
    }

    private void FinishedPickUp()
    {
        SetCanMove(true);
    }

    public void PickUpArtefact()
    {
        StartCoroutine(WaitAndFinishPickUp());
    }
    
    private IEnumerator WaitAndFinishPickUp()
    {
        yield return new WaitForSeconds(5f);
        _animator.SetTrigger("PickUpArtefact");
    }

    private void DestroyArtefact()
    {
        DestroyArtefactEvent.Invoke();
    }

    

}