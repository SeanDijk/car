﻿using UnityEngine;
using UnityEditor;
using System;

public class AutonomousDriveBehaviour : AbstractDriveBehaviour
{
    Vector3 newPosition;
    CarAdvice moveTowardsAdvice;
    public override void StartVector(Car c, float x, float y, float z)
    {
        newPosition = new Vector3(-150, c.transform.position.y, 30);
    }
    public override void GoToNewPosition(Car c, Vector3 newVector)
    {
        newPosition = newVector;
    }
    public override void FixedUpdate(Car c, AbstractSensorBehaviour[] sensorBehaviours)
    {
        //TODO take multple sensorsbehaveiours into account
        CarAdvice finalAdvice = null;
        CarAdvice sensorAdvice = null;
        moveTowardsAdvice = GetMoveTowardsAdvice(c, newPosition);

        for (int i = 0; i < sensorBehaviours.Length; i++)
        {
            var sensorBehaviour = sensorBehaviours[i];
            var carAdvice = sensorBehaviour.DoAction(c);
            sensorAdvice = carAdvice;
        }
        finalAdvice = moveTowardsAdvice;

        if (sensorAdvice != null)
            finalAdvice.Combine(sensorAdvice);


        if (finalAdvice != null)
        {
            Move(c, finalAdvice);
        }

    }

    private void Move(Car car, CarAdvice carAdvice)
    {
        int movetype = carAdvice.MoveType.Second;
        switch (movetype)
        {
            case CarAdvice.ACCELERATE_FW: car.Accelerate(car.MAX_TORQUE_FW); break;
            case CarAdvice.ACCELERATE_BW: car.Accelerate(-car.MAX_TORQUE_BW); break;
            case CarAdvice.INSTANT_BRAKE: car.Brake(car.MAX_TORQUE_BRAKE); break;
            case CarAdvice.KEEP_CURRENT_SPEED: break;
            case CarAdvice.GRADUALLY_BRAKE:
                if (car.GetSpeed() * 2 < car.DrivingSpeed)
                    car.BrakeGradually(car.MAX_TORQUE_BRAKE, 0f);
                break;
        }

        car.Turn(carAdvice.TurnDirection.Second * car.MAX_STEER_ANGLE);
    }
    private void SetCarMaxSpeed(Car c, float f)
    {
        c.DrivingSpeed = f;
    }
    public CarAdvice GetMoveTowardsAdvice(Car c, Vector3 endPos)
    {
        var carPos = c.transform.position;
        float angleTowardsPosition = PositionHelper.GetAngleTowardsPosition(c.transform, endPos);

        var isInfront = IsPositionInFrontOfCar(angleTowardsPosition);
        var enteringCorner = IsAlmostEnteringCorner(c, endPos);
        var turnDirection = GetTurnDirection(c, endPos);

        var movetype = CarAdvice.ACCELERATE_FW;
        if (!isInfront)
            movetype = CarAdvice.ACCELERATE_BW;
        if (enteringCorner)
            movetype = CarAdvice.GRADUALLY_BRAKE;
        return new CarAdvice(movetype, turnDirection);
    }

    public CarAdvice GetMoveToPointAdvice(Car c, Vector3 endPos){
        var advice = GetMoveTowardsAdvice(c, endPos);
        var isCloseToTarget = IsCloseToTarget(c, endPos);

        if (isCloseToTarget)
            advice.Combine(
                new CarAdvice(
                    new AdviceItem<int>(true, CarAdvice.GRADUALLY_BRAKE),
                    new AdviceItem<int>(false, 0)
                ));

        return advice;
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
    private bool IsAlmostEnteringCorner(Car c, Vector3 pos)
    {
        var carPos = c.transform.position;
        if (Vector3.Distance(carPos, pos) > 10f)
            return false;
        if (Vector3.Angle(carPos, pos) <= 20)
            return false;
        return true;
    }

}

