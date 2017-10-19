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

            var position = PositionHelper.GetRelativePosition(car.transform, collider);
            if (position == PositionHelper.POSITION_FRONT)
            {
                objectInFrontOfCar = true;
            }
        }

        if (objectInFrontOfCar)
        {
            return new CarAdvice(
                new AdviceItem<int>(true, CarAdvice.BRAKE),
                new AdviceItem<int>(false, 0)
                );
        }
        //This basicly isnt used since the advice for all items is false
        return new CarAdvice(CarAdvice.ACCELERATE_FW, 0);
    }


    /*
    private int CheckPosition(Car car, Collider collider)
    {
        Vector3 directionToTarget = car.transform.position - collider.transform.position;

        float angel = Vector3.Angle(car.transform.forward, directionToTarget);

        var calculatedAngle = Mathf.Abs(angel);

        if (calculatedAngle > 90) {
            Debug.Log("target is behind me");
            return PositionHelper.POSITION_REAR;
        }

        else if (calculatedAngle <= 90) {
            Debug.Log("target is infront");
            return PositionHelper.POSITION_FRONT;
        }


        return PositionHelper.UNKOWN;
    }
    */
}


