using System;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;
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
    
    private void Start()
    {
        _inputs = GetComponent<PlayerInputs>();
        _characterController = GetComponent<CharacterController>();
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _finalRotationSpeed = rotationSpeed;
        
        _ivyCameraTransform = GameObject.FindGameObjectWithTag("IvyCamera").transform;
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
                        Debug.Log(_inputs.IsRunning);
                        _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
                    }
                }
                else
                {
                    _inputs.JumpIsPressed = false;
                }

                Quaternion inputRotation =
                    Quaternion.LookRotation(new Vector3(_inputs.InputMove.x, 0, _inputs.InputMove.y), Vector3.up);
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
                Debug.Log(_inputs.InputMove);
                if (groundDetector.IsGrounded && _inputs.InputMove.y < 0)
                {
                    //Player can back down
                    Debug.Log("Can back down");
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

    private void SetWasRunning()
    {
        _animator.SetBool("WasRunning",_inputs.IsRunning);
    }
    
    public void SetCanMove(bool canMove)
    {
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
}
