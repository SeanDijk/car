 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Car : MonoBehaviour
{
    public readonly float MAX_TORQUE_FW = 80f;
    public readonly float MAX_TORQUE_BW = 80f;
    public readonly float MAX_TORQUE_BRAKE = 8f * 80f;
    public readonly float MAX_STEER_ANGLE = 45f;

    [Header("General")]
    //The max speed the car is able to drive in km/h
    public float maxSpeed = 45f;
    //A reference to the Gui to update values on screen accordingly
    public SimulationGuiScript guiEditor;

    //The speed the car should Accelerate to.
    private float currentMaxDrivingSpeed;
    public float CurrentMaxDrivingSpeed
    {
        get { return currentMaxDrivingSpeed; }
        set
        {
            if (value > maxSpeed)
                currentMaxDrivingSpeed = maxSpeed;
            else
                currentMaxDrivingSpeed = value;
        }
    }

    //For future version: load this from code so that it can be more dyamic and you dont need the inspector.
    [Header("Wheels")]
    public WheelCollider[] wheelColliders = new WheelCollider[4]; // A list of the wheelcolliders (FL, FR, RL, RR)
    public Transform[] tireMeshes = new Transform[4]; // A list of the tiremeshes (FL, FR, RL, RR)

    /*
     * A triggerbox on the the car that can be used to put trigger scripts on (So that a Drivebehaviour can use triggers)
     */
    private BoxCollider carTriggerBox;
    public BoxCollider CarTriggerBox
    {
        get { return carTriggerBox; }
        private set { carTriggerBox = value; }
    }

    /*
     * Behaviour class for driving
     */
    private AbstractDriveBehaviour driveBehaviour;
    private AbstractDriveBehaviour DriveBehaviour
    {
        get { return driveBehaviour; }
        set {
            driveBehaviour = value;
            driveBehaviour.Initialize(this);
        }
    }

    /*
     * The sensorbehaviours
     */
    private AbstractSensorBehaviour[] sensorBehaviours = new AbstractSensorBehaviour[1];
    private RadarSensorBehaviour radarSensorBehaviour;

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
        //Set reference to the triggerbox
        CarTriggerBox = gameObject.transform.Find("CarTriggerBox").GetComponent<BoxCollider>();

        //Set the drivebehaviour, this should become interchangeable in the future
        DriveBehaviour = new AutonomousDriveBehaviour();
  
        //Change the center of mass, so that this is more to the bottom of the car, to better simulate the actual center of mass of the car.
        GetComponent<Rigidbody>().centerOfMass = gameObject.transform.Find("CenterOfMass").localPosition;

        InitSensors();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMeshesPositions();
        guiEditor.Speed = GetCurrentSpeed();
        guiEditor.AmountOFObjectsInFront = radarSensorBehaviour.GetAmountOfObjects();
    }

    // All the physics must be here
    private void FixedUpdate()
    {
        driveBehaviour.FixedUpdate(this, sensorBehaviours);
    }

    /*
     * Initializes the sensors
     */
    private void InitSensors()
    {
        //Init radar behaviour and link the radars to it.
        radarSensorBehaviour = new RadarSensorBehaviour();
        foreach (var item in GetRadarSensors())
        {
            item.AttachListener(radarSensorBehaviour);
        }
        sensorBehaviours[0] = radarSensorBehaviour;
    }
    private Radar[] GetRadarSensors()
    {
        var sensorObjectsParent = gameObject.transform.Find("Sensors/RadarSensors");
        var radarArray = new Radar[sensorObjectsParent.childCount];

        for (int i = 0; i < sensorObjectsParent.childCount; i++)
        {
            radarArray[i] = sensorObjectsParent.GetChild(i).gameObject.GetComponent<Radar>();
        }

        return radarArray;
    }

    /*
     * Turns the front wheels.
     * Postive angle -> Turn left
     * Negtive angle => Turn right
     */
    public void Turn(float angle)
    {
        //Make sure the angle isnt bigger than max or smaller than min
        if (angle > MAX_STEER_ANGLE)
            angle = MAX_STEER_ANGLE;
        else if (angle < -MAX_STEER_ANGLE)
            angle = -MAX_STEER_ANGLE;

        var direction = angle / Mathf.Abs(angle);

        for (int i = 0; i < 2; i++)
        {
            
            //wheelColliders[i].steerAngle += direction * 10;
            //if (wheelColliders[i].steerAngle > angle)
            //    wheelColliders[i].steerAngle = angle;
            //if (wheelColliders[i].steerAngle < -angle)
            //    wheelColliders[i].steerAngle = -angle;
                
            wheelColliders[i].steerAngle = angle;
        }

    }

    public void Accelerate(float torque)
    {
        //If the lower than the max speed, accelerate, else stop accelerating
        if (GetCurrentSpeed() <= maxSpeed)
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
        else
        {
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                wheelColliders[i].motorTorque = 0;
            }
        }
    }

    public void Brake(float torque)
    {
        if (torque > MAX_TORQUE_BRAKE)
            torque = MAX_TORQUE_BRAKE;
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            //wheelColliders[i].motorTorque = 0;
            wheelColliders[i].brakeTorque = torque;
        }
    }

    //Brakes gradually, needs improvement
    public void BrakeGradually(float torque, float distance)
    {
        Brake(wheelColliders[0].brakeTorque + 10 * Time.deltaTime);
    }


    /*
     * Gets the current speed in km/h
     */
    public float GetCurrentSpeed()
    {
        var speed =  gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
        return speed;
    }
    /*
     * Brakes if the car is above a certain speed.
     */
    private void BrakeUntilSpeed(float speed)
    {
        if (GetCurrentSpeed() > speed)
            Brake(MAX_TORQUE_BRAKE);
    }
}





