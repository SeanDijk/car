using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class RadarSensorBehaviour : AbstractSensorBehaviour
{
    private List<Collider> currentVisableColliders = new List<Collider>(); 
    private static Logger logger; //new Logger("logger/logTest.csv");


    public override void Initialize()
    {
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!currentVisableColliders.Contains(other))
        {
            SetLogger();
            currentVisableColliders.Add(other);
           // Debug.Log(Time.time.ToString() + " Enter " + currentVisableColliders.Count);
            logger.AddLineToBuffer(other.gameObject.name, "Enter", Time.realtimeSinceStartup.ToString());
            logger.Commit();
        }

    }

    public void OnTriggerExit(Collider other)
    {
        SetLogger();
        currentVisableColliders.Remove(other);
        //Debug.Log(Time.time.ToString() + " Exit " + currentVisableColliders.Count);
        logger.AddLineToBuffer(other.gameObject.name, "Exit", Time.realtimeSinceStartup.ToString());
        logger.Commit();
    }

    public override CarAdvice GiveAdvice(Car car)
    {
        // Debug.Log("RadarSensorBehaviour action");

        bool objectInFrontOfCar = false;
        bool objectLeftFront = false;
        bool objectRightFront = false;

        for (int i = 0; i < currentVisableColliders.Count; i++)
        {
            var collider = currentVisableColliders[i];

            var position = PositionHelper.GetRelativePosition(car.transform, collider);
            if (position == PositionHelper.POSITION_FRONT && PositionHelper.IsCloseTo(car.transform, collider, 15f))
                objectInFrontOfCar = true;
            if (position == PositionHelper.POSITION_FRONT_LEFT)
                objectLeftFront = true;
            if (position == PositionHelper.POSITION_FRONT_RIGHT)
                objectRightFront = true;
        }
        AdviceItem<int> move;
        AdviceItem<int> turn;

        if (objectInFrontOfCar)
            move = new AdviceItem<int>(true, CarAdvice.INSTANT_BRAKE);
        else
            move = new AdviceItem<int>(false, CarAdvice.INSTANT_BRAKE);

        /*if (objectLeftFront ^ objectRightFront)
        {
            if (objectLeftFront)
                turn = new AdviceItem<int>(true, CarAdvice.TURN_RIGHT);
            else
                turn = new AdviceItem<int>(true, CarAdvice.TURN_LEFT);
        }
        else
        {
            turn = new AdviceItem<int>(false, CarAdvice.INSTANT_BRAKE);
        }*/

        turn = new AdviceItem<int>(false, CarAdvice.TURN_NONE);
        return new CarAdvice(move, turn);

    }
    private void SetLogger()
    {
        if (logger == null)
        {
            System.IO.Directory.CreateDirectory("logger");
            logger = new Logger("logger/logTest.csv");
        }
    }

    public int GetAmountOfObjects()
    {
        return currentVisableColliders.Count;
    }
}


