using UnityEngine;
using UnityEditor;

public abstract class AbstractDriveBehaviour
{
    public abstract void FixedUpdate(Car c, float maxTorqueFw, float maxTorqueBw, float maxTorqueBrake, float maxSteerAngle, AbstractSensorBehaviour[] sensorBehaviours);
    public abstract void goToNewPosition(Car c, Vector3 newVector);
    public abstract void startVector(Car c, float x, float y, float z);

}