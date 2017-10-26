using UnityEngine;
using UnityEditor;

public abstract class AbstractDriveBehaviour
{
    private bool isInitialized = false;
    public void Initialize(Car c)
    {
        if (!isInitialized)
        {
            InitializeImplementation(c);
            isInitialized = true;
        }
        else
        {
            Debug.Log(this.ToString() + "has already been initialized");
        }
    }


    public abstract void FixedUpdate(Car c, AbstractSensorBehaviour[] sensorBehaviours);

    protected abstract void InitializeImplementation(Car c);
}