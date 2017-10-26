using UnityEngine;
using UnityEditor;
using System;

public class PlayerDriveBehaviour : AbstractDriveBehaviour
{
    public override void FixedUpdate(Car c, AbstractSensorBehaviour[] sensorBehaviours)
    {
        float steer = Input.GetAxis("Horizontal");
        float accelerate = Input.GetAxis("Vertical");
        float finalAngle = steer * 45f;

        c.Turn(finalAngle);
        c.Accelerate(accelerate * c.MAX_TORQUE_FW);
    }

    protected override void InitializeImplementation(Car c)
    {
        
    }
}