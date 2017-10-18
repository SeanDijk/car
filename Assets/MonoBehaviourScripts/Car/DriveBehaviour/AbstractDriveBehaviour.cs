using UnityEngine;
using UnityEditor;

public abstract class AbstractDriveBehaviour
{
    public abstract void FixedUpdate(Car c, AbstractSensorBehaviour[] sensorBehaviours);
    public abstract void GoToNewPosition(Car c, Vector3 newVector);
    public abstract void StartVector(Car c, float x, float y, float z);

}