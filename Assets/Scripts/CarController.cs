using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float maxSteerAngle = 100f;
    [SerializeField] float speed = 10f, rotationSpeed = 10f;

    [SerializeField] WheelCollider frontRWheel;
    [SerializeField] WheelCollider frontLWheel;
    [SerializeField] WheelCollider backLWheel;
    [SerializeField] WheelCollider backRWheel;

    private void Drive()
    {
        float zAxis = Input.GetAxis("Vertical");
        Vector3 acceleration = transform.forward * (zAxis * speed);
        rb.AddForce(acceleration);
        float xAxis = Input.GetAxis("Horizontal");
        transform.Rotate(xAxis * Vector3.up * rotationSpeed);
    }

    private void FixedUpdate()
    {
        Drive();
        /*HandleMotor();
        HandleSteering();
        UpdateWheels();*/
    }

    private void HandleMotor()
    {
        float zAxis = Input.GetAxis("Vertical");
        frontRWheel.motorTorque = zAxis * speed;
        frontLWheel.motorTorque = zAxis * speed;
    }

    void HandleSteering()
    {
        float xAxis = Input.GetAxis("Horizontal");
        var steerAngle = xAxis * maxSteerAngle;
        frontLWheel.steerAngle = steerAngle;
        frontRWheel.steerAngle = steerAngle;
    }

    void UpdateWheels()
    {
        UpdateSingleWheel(frontLWheel, frontLWheel.transform);
        UpdateSingleWheel(frontRWheel, frontRWheel.transform);
        UpdateSingleWheel(backLWheel, backLWheel.transform);
        UpdateSingleWheel(backLWheel, backLWheel.transform);
    }

    void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
