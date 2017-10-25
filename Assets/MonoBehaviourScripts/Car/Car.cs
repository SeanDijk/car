 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Car : MonoBehaviour
{
    //private static float MAX_TORQUE_FW = 500f;
    public readonly float MAX_TORQUE_FW = 80f;
    public readonly float MAX_TORQUE_BW = 80f;
    public readonly float MAX_TORQUE_BRAKE = 8f * 80f;
    public readonly float MAX_STEER_ANGLE = 45f;

    private AbstractDriveBehaviour driveBehaviour = new AutonomousDriveBehaviour(); //Should be interchangeable

    [Header("General")]
    public Transform centerOfMass;
    public float maxSpeed = 45f; // km/h
    public SimulationGuiScript guiEditor;

    //The speed the car should Accelerate to.
    private float drivingSpeed;
    public float DrivingSpeed
    {
        get { return drivingSpeed; }
        set
        {
            if (value > maxSpeed)
                drivingSpeed = maxSpeed;
            else
                drivingSpeed = value;
        }
    }

    private Rigidbody mRigidbody;

    [Header("Sensors")]
    private AbstractSensorBehaviour[] sensorBehaviours = new AbstractSensorBehaviour[1];
    public RadarSensorBehaviour radarSensorBehaviour;// = new RadarSensorBehaviour();

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
        driveBehaviour.StartVector(this, -85f, 0f, -100f);
        sensorBehaviours[0] = radarSensorBehaviour;


        foreach (var item in sensorBehaviours)
        {
            item.Initialize();
        }



    }

    // Update is called once per frame
    void Update()
    {
        UpdateMeshesPositions();
        guiEditor.Speed = GetSpeed();
        guiEditor.AmountOFObjectsInFront = radarSensorBehaviour.GetAmountOfObjects();
    }

    // All the physics must be here
    private void FixedUpdate()
    {
        driveBehaviour.FixedUpdate(this, sensorBehaviours);
        //GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody>().velocity, maxSpeed);
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
        if (GetSpeed() <= maxSpeed)
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

    public void BrakeUntilSpeed(float speed)
    {
        if (GetSpeed() > speed)
            Brake(MAX_TORQUE_BRAKE);

    }

    public void ChangeDirection(Vector3 newVector) // Changes direction of the car.
    {
        driveBehaviour.GoToNewPosition(this, newVector);
    }

    public float GetSpeed() //km/h
    {
        var speed =  gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
        //Debug.Log(speed);
        return speed;
    }

    /*public float GetBrakeDistance()
    {
        var rigedBody = GetComponent<Rigidbody>();
        var distance = Mathf.Pow(rigedBody.velocity.magnitude, 2) / (2f * 0.8f) * 9.81f;
        return distance;
    }*/
}





