﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Car : MonoBehaviour
{
    //private static float MAX_TORQUE_FW = 500f;
    private static float MAX_TORQUE_FW = 500f;
    private static float MAX_TORQUE_BW = 250F;

    private static float MAX_TORQUE_BRAKE = 4f*MAX_TORQUE_FW;

    private static float MAX_STEER_ANGLE = 45f;

    private AbstractDriveBehaviour driveBehaviour = new AutonomousDriveBehaviour();

    //private DriveBehaviour driveBehaviour = new PlayerDriveBehaviour();

    [Header("General")]
    public Transform centerOfMass;
    private Rigidbody mRigidbody;

    [Header("Sensors")]
    private AbstractSensorBehaviour[] sensorBehaviours = new AbstractSensorBehaviour[1];
    public RadarSensorBehaviour radarSensorBehaviour = new RadarSensorBehaviour();

    [Header("Wheels")]
    public WheelCollider[] wheelColliders = new WheelCollider[4]; // A list of the wheelcolliders (FL, FR, RL, RR)
    public Transform[] tireMeshes = new Transform[4]; // A list of the tiremeshes (FL, FR, RL, RR)

    /*
     * Updates the wheel rotation and position according to the wheelcolliders
     */
    void UpdateMeshesPositions()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            Quaternion quaternion;
            Vector3 position;
            wheelColliders[i].GetWorldPose(out position, out quaternion);

            tireMeshes[i].position = position;
            tireMeshes[i].rotation = quaternion;
        }
    }

    // Use this for initialization
    void Start()
    {
        //Change the center of mass, so the car doesnt flip while turning
        mRigidbody = GetComponent<Rigidbody>();
        mRigidbody.centerOfMass = centerOfMass.localPosition;
        driveBehaviour.startVector(this, -85f, 0f, -100f);
        sensorBehaviours[0] = radarSensorBehaviour;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateMeshesPositions();
    }
    /*
     * All the physics must be here
     */
    private void FixedUpdate()
    {
        driveBehaviour.FixedUpdate(this, MAX_TORQUE_FW, MAX_TORQUE_BW, MAX_TORQUE_BRAKE, MAX_STEER_ANGLE, sensorBehaviours);
    }


    /*
     * Turns the front wheels.
     * Postive angle -> Turn left
     * Negtive angle => Turn right
     */
    public void Turn(float angle)
    {
        if (angle > MAX_STEER_ANGLE)
            angle = MAX_STEER_ANGLE;
        else if (angle < -MAX_STEER_ANGLE)
            angle = -MAX_STEER_ANGLE;

        wheelColliders[0].steerAngle = angle;
        wheelColliders[1].steerAngle = angle;
    }

    public void Accelerate(float torque)
    {
        if (torque > MAX_TORQUE_FW)
            torque = MAX_TORQUE_FW;
        else if (torque < -MAX_TORQUE_BW)
            torque = -MAX_TORQUE_BW;

        for (int i = 0; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].motorTorque = -torque;
            wheelColliders[i].brakeTorque = 0;
        }
    }

    public void Brake(float torque)
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            //wheelColliders[i].motorTorque = 0;
            wheelColliders[i].brakeTorque = torque;
        }
    }

    public void ChangeDirection(Vector3 newVector) // Changes direction of the car.
    {
        driveBehaviour.goToNewPosition(this, newVector);
    }
}




