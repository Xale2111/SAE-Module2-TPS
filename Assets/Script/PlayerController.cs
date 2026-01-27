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
    
    [SerializeField][Range(0,1)] private float rotationSpeed = 0.05f;
    [SerializeField] private GroundDetector[] groundDetector;
    
    private PlayerInputs _inputs;
    private CharacterController _characterController;
    private Animator _animator;

    private float _verticalVelocity;

    private Camera _mainCamera;

    bool isGrounded;

    private void Start()
    {
        _inputs = GetComponent<PlayerInputs>();
        _characterController = GetComponent<CharacterController>();
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float moveMagnitude = _inputs.InputMove.magnitude;
        
        Vector3 horizontalVelocity = _inputs.InputIsRunning ? transform.forward * (moveMagnitude * runSpeed) : transform.forward * (moveMagnitude * walkSpeed);

        foreach (GroundDetector detector in groundDetector)
        {
            isGrounded = false;
            if (detector.IsGrounded)
            {
                isGrounded = true;
                break;
            }
        }

        _verticalVelocity = !isGrounded ? _verticalVelocity + Physics.gravity.y * Time.deltaTime : 0;


        if (isGrounded)
        {
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
        
        Quaternion rotation = Quaternion.Euler(0,cameraRotation.eulerAngles.y,0)*inputRotation;
        
        
        
        _characterController.Move((horizontalVelocity + new Vector3(0,_verticalVelocity,0))*Time.deltaTime);

        if (horizontalVelocity.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,rotation,rotationSpeed);
        }
        
        _animator.SetFloat("AbsVelocity",moveMagnitude);
        _animator.SetBool("IsRunning",_inputs.InputIsRunning);
    }
}
