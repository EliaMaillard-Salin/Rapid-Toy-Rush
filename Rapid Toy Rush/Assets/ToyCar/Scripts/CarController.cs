using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody carRb;
    [SerializeField] private Transform[] rayPoints;
    [SerializeField] private LayerMask drivableLayer;
    [SerializeField] private Transform accelerationPoint;
    [SerializeField] private GameObject[] tires = new GameObject[4];
    [SerializeField] private GameObject[] frontTiresParent = new GameObject[2];
    [SerializeField] private TrailRenderer[] skidmarksTR = new TrailRenderer[2];
    
    [Header("Suspension Settings")]
    [SerializeField] private float suspensionLength;
    [SerializeField] private float wheelRadius;
    
    [Header("Car Settings")]
    [SerializeField] private float acceleration = 25f;
    [SerializeField] private float deceleration = 100f;
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float turnStrength = 15f;
    [SerializeField] private AnimationCurve turningCurve;
    
    [Header("Visuals")] 
    [SerializeField] private float tireRotSpeed = 3000f;
    [SerializeField] private float maxSteeringAngle = 30f;
    [SerializeField] private float skidmarkMinVelocity = 10;
    
    [Header("Input")] 
    [SerializeField] private InputAction moveAxis;
    [SerializeField] private InputAction steerAxis;

    
    private float _moveInput = 0;
    private float _steerInput = 0;
    
    private bool[] _wheelsIsGrounded = new bool[4];
    private bool _isGrounded = false;
    
    private Vector3 _currentLocalVelocity = Vector3.zero;
    private float _carVelocityRatio;
    
    private void Start()
    {
        carRb = GetComponent<Rigidbody>();
        moveAxis.Enable();
        steerAxis.Enable();
    }

    private void FixedUpdate()
    {
        GetPlayerInput();
        Suspension();
        GroundCheck();
        CalculateCarVelocity();
        Movement();
        
        TireVisuals();
        SkidMarks();
    }

    private void Suspension()
    {
        for (int i = 0; i < rayPoints.Length; i++)
        {
            float totalLength = suspensionLength + wheelRadius;
            RaycastHit hit;
            if (Physics.Raycast(rayPoints[i].position, -rayPoints[i].up, out hit, totalLength , drivableLayer))
            {
                _wheelsIsGrounded[i] = true;
            }
            else
            {
                _wheelsIsGrounded[i] = false;
            }
        }
    }

    private void GroundCheck()
    {
        int tempGroundedWheels = 0;

        for (int i = 0; i < _wheelsIsGrounded.Length; i++)
        {
            if (_wheelsIsGrounded[i]) tempGroundedWheels++;
        }
        
        _isGrounded = tempGroundedWheels > 1;
    }

    private void CalculateCarVelocity()
    {
        _currentLocalVelocity = transform.InverseTransformDirection(carRb.velocity);
        _carVelocityRatio = _currentLocalVelocity.z / maxSpeed;
    }
    
    private void GetPlayerInput()
    {
        _moveInput = moveAxis.ReadValue<float>();
        _steerInput = steerAxis.ReadValue<float>();
    }

    private void Acceleration()
    {
        carRb.AddForceAtPosition(acceleration * _moveInput * transform.forward, accelerationPoint.position, ForceMode.Acceleration);
    }

    private void Deceleration()
    {
        carRb.AddForceAtPosition(deceleration * _moveInput * -transform.forward, accelerationPoint.position, ForceMode.Acceleration);
    }

    private void Turn()
    {
        carRb.AddRelativeTorque(turnStrength * _steerInput * turningCurve.Evaluate(Mathf.Abs(_carVelocityRatio)) * Math.Sign(_carVelocityRatio) * carRb.transform.up, ForceMode.Acceleration);
    }

    private void Movement()
    {
        if (_isGrounded)
        {
            Acceleration();
            Deceleration();
            Turn();
        }
    }

    private void TireVisuals()
    {
        float steeringAngle = maxSteeringAngle * -_steerInput;
        
        for (int i = 0; i < tires.Length; i++)
        {
            if (i < 2)
            {
                tires[i].transform.Rotate(Vector3.right, -tireRotSpeed * _moveInput * Time.deltaTime, Space.Self);
                
                var rot = frontTiresParent[i].transform.localEulerAngles;
                frontTiresParent[i].transform.localEulerAngles = new Vector3(rot.x, steeringAngle, rot.z);
            }
            else tires[i].transform.Rotate(Vector3.right, -tireRotSpeed * _carVelocityRatio * Time.deltaTime, Space.Self);
        }
    }

    private void SkidMarks()
    {
        ToggleSkidMarks(_isGrounded && Mathf.Abs(_currentLocalVelocity.x) > skidmarkMinVelocity);
    }

    private void ToggleSkidMarks(bool toggle)
    {
        foreach (var skidMark in skidmarksTR)
        {
            skidMark.emitting = toggle;
        }
    }
}
