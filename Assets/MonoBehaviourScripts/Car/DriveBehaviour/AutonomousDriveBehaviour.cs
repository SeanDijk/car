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
    public override void goToNewPosition(Car c, float x, float y, float z)
    {
        newPosition = new Vector3(-150, c.transform.position.y, -100);
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
        finalAdvice.ShouldBrake = sensorAdvice.ShouldBrake;
        finalAdvice.ShouldAccelerate = sensorAdvice.ShouldAccelerate;

        Debug.Log(finalAdvice);


        if (finalAdvice != null)
        {
            if (finalAdvice.ShouldAccelerate)
                c.Accelerate(maxTorqueFw);
            if (finalAdvice.ShouldBrake)
                c.Brake(maxTorqueBrake);
            if (finalAdvice.ShouldReverse)
                c.Accelerate(-maxTorqueBw);

            c.Turn(finalAdvice.TurnDirection * maxSteerAngle);
        }

    }

    public CarAdvice MoveTowards(Car c, Vector3 endPos)
    {

        var carPos = c.transform.position;
        float angleTowardsPosition = GetAngleTowardsPosition(c, endPos);

        var isInfront = IsPositionInFrontOfCar(angleTowardsPosition);
        var turnDirection = GetTurnDirection(c, endPos);
        var isCloseToTarget = IsCloseToTarget(c, endPos);
        return new CarAdvice(isInfront, isCloseToTarget, !isInfront, turnDirection);


    }
    private bool IsPositionInFrontOfCar(float angle)
    {
        return Mathf.Abs(angle) < 90;
    }
    private bool IsCloseToTarget(Car c, Vector3 pos)
    {
        //TODO should do a calculation with velocity and such
        return Vector3.Distance(c.transform.position, pos) < 20f;
    }
    private float GetAngleTowardsPosition(Car c, Vector3 pos)
    {
        Vector3 directionToTarget = c.transform.position - pos;
        return Vector3.Angle(c.transform.forward, directionToTarget);
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


