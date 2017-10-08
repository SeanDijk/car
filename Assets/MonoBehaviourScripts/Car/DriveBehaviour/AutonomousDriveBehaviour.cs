using UnityEngine;
using UnityEditor;
using System;

public class AutonomousDriveBehaviour : AbstractDriveBehaviour
{
    Vector3 newPosition;
    CarAdvice moveTowardsAdvice;
    public override void startVector(Car c, float x, float y, float z)
    {
        newPosition = new Vector3(-85, c.transform.position.y, -100);
    }
    public override void goToNewPosition(Car c, Vector3 newVector)
    {
        newPosition = newVector;
    }
    public override void FixedUpdate(Car c, float maxTorqueFw, float maxTorqueBw, float maxTorqueBrake, float maxSteerAngle, AbstractSensorBehaviour[] sensorBehaviours)
    {
        Debug.Log(newPosition.x);
        //TODO take multple sensorsbehaveiours into account
        CarAdvice finalAdvice = null;
        CarAdvice sensorAdvice = null;
        moveTowardsAdvice = MoveTowards(c, newPosition);

        for (int i = 0; i < sensorBehaviours.Length; i++)
        {
            var sensorBehaviour = sensorBehaviours[i];
            var carAdvice = sensorBehaviour.DoAction(c);
            sensorAdvice = carAdvice;
        }
        finalAdvice = moveTowardsAdvice;

        if (sensorAdvice != null)
            finalAdvice.Combine(sensorAdvice);


        Debug.Log(finalAdvice);


        if (finalAdvice != null)
        {
            int movetype = finalAdvice.MoveType.Second;
            Debug.Log("movetype " + movetype);
            switch (movetype)
            {
                case CarAdvice.ACCELERATE_FW: c.Accelerate(maxTorqueFw); break;
                case CarAdvice.ACCELERATE_BW: c.Accelerate(-maxTorqueBw); break;
                case CarAdvice.BRAKE: c.Brake(maxTorqueBrake); break;
            }

            c.Turn(finalAdvice.TurnDirection.Second * maxSteerAngle);
        }

    }

    public CarAdvice MoveTowards(Car c, Vector3 endPos)
    {

        var carPos = c.transform.position;
        float angleTowardsPosition = PositionHelper.GetAngleTowardsPosition(c.transform, endPos);

        var isInfront = IsPositionInFrontOfCar(angleTowardsPosition);
        var turnDirection = GetTurnDirection(c, endPos);
        var isCloseToTarget = IsCloseToTarget(c, endPos);


        var movetype = CarAdvice.ACCELERATE_FW;
        if (!isInfront)
            movetype = CarAdvice.ACCELERATE_BW;
        if (isCloseToTarget)
            movetype = CarAdvice.BRAKE;

        return new CarAdvice(movetype, turnDirection);



    }
    private bool IsPositionInFrontOfCar(float angle)
    {
        return Mathf.Abs(angle) < 90;
    }
    private bool IsCloseToTarget(Car c, Vector3 pos)
    {
        //TODO should do a calculation with velocity and such
        return Vector3.Distance(c.transform.position, pos) < 10f;
    }
    private int GetTurnDirection(Car c, Vector3 pos) //dont turn = 0 left = -1 right =1
    {
        var offset = 3;

        var dir = c.transform.InverseTransformPoint(pos).x;
        if (dir > offset)
        {
            return CarAdvice.TURN_LEFT;
        }
        else if (dir < -offset)
        {
            return CarAdvice.TURN_RIGHT;
        }
        else
        {
            return CarAdvice.TURN_NONE;
        }
    }


}


