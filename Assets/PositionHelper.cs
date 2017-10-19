using UnityEngine;
using System.Collections;

public class PositionHelper
{
    public const int POSITION_FRONT_LEFT = 1;
    public const int POSITION_FRONT = 2;
    public const int POSITION_FRONT_RIGHT = 3;
    public const int POSITION_LEFT = 4;
    public const int POSITION_RIGHT = 5;
    public const int POSITION_REAR_LEFT = 6;
    public const int POSITION_REAR = 7;
    public const int POSITION_REAR_RIGHT = 8;
    public const int UNKOWN = 9;


    public static float GetAngleTowardsPosition(Transform transform, Vector3 pos)
    {
        Vector3 directionToTarget = transform.position - pos;
        return Vector3.Angle(transform.forward, directionToTarget);
    }

    public static int IsPositionInFrontOfObject(Transform baseObject, float angleTowardsPosition)
    {
        var absoluteAngle = Mathf.Abs(angleTowardsPosition);

        if (absoluteAngle < 90)
            return POSITION_FRONT;
        else
            return POSITION_REAR;
    }



    public static int GetRelativePosition(Transform baseObject, Collider relativeObject)
    {
        //var bounds = baseObject.GetComponent<Renderer>().bounds.size;

        var offsetLeftRight = 5;//bounds.z;
        //var offsetFrontBack = //bounds.x;

        var leftOrRight = baseObject.InverseTransformPoint(relativeObject.transform.position).x;
        var frontOrBack = IsPositionInFrontOfObject(baseObject, GetAngleTowardsPosition(baseObject, relativeObject.transform.position));

        if (leftOrRight > offsetLeftRight)
        {
            if (frontOrBack == POSITION_FRONT)
                return POSITION_FRONT_LEFT;
            else if (frontOrBack == POSITION_REAR)
                return POSITION_REAR_LEFT;
            else
                return POSITION_LEFT;
        }
        else if (leftOrRight < -offsetLeftRight)
        {
            if (frontOrBack == POSITION_FRONT)
                return POSITION_FRONT_RIGHT;
            else if (frontOrBack == POSITION_REAR)
                return POSITION_REAR_RIGHT;
            else
                return POSITION_RIGHT;
        }
        else
        {
            return frontOrBack;
        }



    }

}