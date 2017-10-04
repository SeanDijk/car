using UnityEngine;
using UnityEditor;

public abstract class AbstractDriveBehaviour
{
    public abstract void FixedUpdate(Car c, float maxTorqueFw, float maxTorqueBw, float maxTorqueBrake, float maxSteerAngle, AbstractSensorBehaviour[] sensorBehaviours);
}