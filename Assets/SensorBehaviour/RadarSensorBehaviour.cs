using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class RadarSensorBehaviour : AbstractSensorBehaviour
{
    public Radar[] radars = new Radar[4];
    static List<Collider> currentVisableColliders = new List<Collider>(); //TODO shouldnt be static but is a hotfix for now that works while having just one car.

    public override void Initialize()
    {
        for (int i = 0; i < radars.Length; i++)
        {
            radars[i].myListener = this;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!currentVisableColliders.Contains(other))
        {
            currentVisableColliders.Add(other);
            Debug.Log(Time.time.ToString() + " Enter " + currentVisableColliders.Count);
        }

    }

    public void OnTriggerExit(Collider other)
    {
        currentVisableColliders.Remove(other);
        Debug.Log(Time.time.ToString() + " Exit " + currentVisableColliders.Count);
    }

    public override CarAdvice DoAction(Car car)
    {
        // Debug.Log("RadarSensorBehaviour action");

        bool objectInFrontOfCar = false;

        for (int i = 0; i < currentVisableColliders.Count; i++)
        {
            var collider = currentVisableColliders[i];

            var position = CheckPosition(car, collider);
            Debug.Log("Position = " + position.ToString());
            if (position == POSITION_FRONT)
            {
                objectInFrontOfCar = true;
            }
        }

        return new CarAdvice(!objectInFrontOfCar, objectInFrontOfCar, false, 0);
    }

    private static int POSITION_FRONT_LEFT = 1;
    private static int POSITION_FRONT = 2;
    private static int POSITION_FRONT_RIGHT = 3;
    private static int POSITION_LEFT= 4;
    private static int POSITION_RIGHT = 5;
    private static int POSITION_REAR_LEFT = 6;
    private static int POSITION_REAR = 7;
    private static int POSITION_REAR_RIGHT = 8;
    private static int UNKOWN = 9;

    private int CheckPosition(Car car, Collider collider)
    {
        Vector3 directionToTarget = car.transform.position - collider.transform.position;

        float angel = Vector3.Angle(car.transform.forward, directionToTarget);

        var calculatedAngle = Mathf.Abs(angel);

        if (calculatedAngle > 90) {
            Debug.Log("target is behind me");
            return POSITION_REAR;
        }

        else if (calculatedAngle <= 90) {
            Debug.Log("target is infront");
            return POSITION_FRONT;
        }


        return UNKOWN;
    }
}


