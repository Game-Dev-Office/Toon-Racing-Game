using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    public float maxSpeed = -15f;
    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;

    private bool isBreaking;
    private bool isBoost;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    Rigidbody rb;
    void Start()
    {
        isBoost = false;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        frontLeftWheelCollider.motorTorque =  motorForce;
        frontRightWheelCollider.motorTorque =  motorForce;
        rearLeftWheelCollider.motorTorque = motorForce;
        rearRightWheelCollider.motorTorque = motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();

        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }

        GetInput();
       // HandleMotor();
        HandleSteering();
        UpdateWheels();

        //if (rb.velocity.z <= -15f)
        //{
        //    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, speed);
        //}
      

        if (!isBreaking && rb.velocity.z > -4f)
        {
            TurboBoost();
        }
    }
    
   public void Boosting()
    {
        StartCoroutine(Boost2());

        //if (isBoost == true)
        //{
        //    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z * 2f);
        //    //Vector3
        //    //rb.velocity( rb.velocity.x, rb.velocity.y, rb.velocity.z *2f);
        //}
    }
    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
        rearRightWheelCollider.motorTorque = verticalInput * motorForce;

        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
       
    }

    private void HandleSteering()
    {
       // currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot; 
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    public void TurboBoost()
    {
        StartCoroutine(Boost());
    }

    IEnumerator Boost()
    {
        isBoost = true;
        motorForce = 500f;

        yield return new WaitForSeconds(2f);
        motorForce = 100f;
        isBoost = false;
    }
    IEnumerator Boost2()
    {
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z * 2f);
        // button deaktif
        yield return new WaitForSeconds(2f);
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -15f);
        // button aktif
    }
}
