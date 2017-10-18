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
    public override void GoToNewPosition(Car c, Vector3 newVector)
    {
        throw new NotImplementedException();
    }

    public override void StartVector(Car c, float x, float y, float z)
    {
        throw new NotImplementedException();
    }

}