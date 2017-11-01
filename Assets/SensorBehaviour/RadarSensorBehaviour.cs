using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class RadarSensorBehaviour : AbstractSensorBehaviour
{
    private List<Collider> currentVisableColliders = new List<Collider>(); 
    private static Logger logger; 

    /*
     * If the collider isnt seen already, log the name and time and add it to seen objects.
     */
    public void OnTriggerEnter(Collider other)
    {
        if (!currentVisableColliders.Contains(other))
        {
            SetLogger();
            currentVisableColliders.Add(other);
            logger.AddLineToBuffer(other.gameObject.name, "Enter", Time.realtimeSinceStartup.ToString());
            logger.Commit();
        }

    }
    /*
     * Log and remove the colider from the list.
     */
    public void OnTriggerExit(Collider other)
    {
        if (currentVisableColliders.Contains(other))
        {
            SetLogger();
            currentVisableColliders.Remove(other);
            logger.AddLineToBuffer(other.gameObject.name, "Exit", Time.realtimeSinceStartup.ToString());
            logger.Commit();
        }
    }



    public override CarAdvice GiveAdvice(Car car)
    {
        bool objectInFrontOfCar = false;
        bool objectLeftFront = false;
        bool objectRightFront = false;

        //Check if objects are infront and on the left and right side infront of the car
        for (int i = 0; i < currentVisableColliders.Count; i++)
        {
            var collider = currentVisableColliders[i];

            var position = PositionHelper.GetRelativePosition(car.transform, collider);
            if (position == PositionHelper.POSITION_FRONT && PositionHelper.IsCloseTo(car.transform, collider, 45f))
                objectInFrontOfCar = true;
            if (position == PositionHelper.POSITION_FRONT_LEFT)
                objectLeftFront = true;
            if (position == PositionHelper.POSITION_FRONT_RIGHT)
                objectRightFront = true;
        }
        AdviceItem<int> move;
        AdviceItem<int> turn;

        if (objectInFrontOfCar) //If there is an object infront of the car, brake otherwise dont give an advice.
            move = new AdviceItem<int>(true, CarAdvice.INSTANT_BRAKE);
        else
            move = new AdviceItem<int>(false, CarAdvice.INSTANT_BRAKE);

        //(Not finished) Some code that could be used for object evaision, but eventually we didnt need this.
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


