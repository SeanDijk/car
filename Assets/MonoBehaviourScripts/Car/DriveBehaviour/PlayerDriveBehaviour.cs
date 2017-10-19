﻿using UnityEngine;
using UnityEditor;
using System;

public class PlayerDriveBehaviour : AbstractDriveBehaviour
{
    public override void FixedUpdate(Car c, float maxTorqueFw, float maxTorqueBw, float maxTorqueBrake, float maxSteerAngle, AbstractSensorBehaviour[] sensorBehaviours)
    {
        float steer = Input.GetAxis("Horizontal");
        float accelerate = Input.GetAxis("Vertical");
        float finalAngle = steer * 45f;

        c.Turn(finalAngle);
        c.Accelerate(accelerate * maxTorqueFw);
    }
    public override void goToNewPosition(Car c, Vector3 newVector)
    {
        throw new NotImplementedException();
    }

    public override void startVector(Car c, float x, float y, float z)
    {
        throw new NotImplementedException();
    }

}