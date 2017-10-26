using UnityEngine;
using UnityEditor;
/*
 * This class is used to give 'advice' the AutomonousCarBehaviour. 
 */ 
public class CarAdvice
{
    //Turning values
    public const int TURN_LEFT = -1;
    public const int TURN_RIGHT = 1;
    public const int TURN_NONE = 0;

    //Moving values
    public const int KEEP_CURRENT_SPEED = -1;
    public const int ACCELERATE_FW = 0;
    public const int ACCELERATE_BW = 1;
    public const int GRADUALLY_BRAKE = 2;
    public const int INSTANT_BRAKE = 3;

    /*
     * Item that determines if and how the car should move.
     */
    private AdviceItem<int> moveType;
    public AdviceItem<int> MoveType
    {
        get { return moveType; }
        set { moveType = value; }
    }

    /*
     * Item that determines if and how the car should turn.
     */
    private AdviceItem<int> turnDirection;
    public AdviceItem<int> TurnDirection
    {
        get { return turnDirection; }
        set { turnDirection = value; }
    }

    public CarAdvice(AdviceItem<int> moveType, AdviceItem<int> turnDirection)
    {
        MoveType = moveType;
        TurnDirection = turnDirection;
    }

    public CarAdvice(int moveType, int turnDirection)
    {
        MoveType = new AdviceItem<int>(false, moveType);
        TurnDirection = new AdviceItem<int>(false, turnDirection);
    }

    /*
     * Combines two advices. If an advice from 'other' is true, it will overwrite that of 'this'. Otherwise nothing happens.
     */ 
    public void Combine(CarAdvice other)
    {
        if (other.MoveType.UseAdvice)
            MoveType = other.MoveType;
        if (other.TurnDirection.UseAdvice)
            TurnDirection = other.turnDirection;
    }

}

public class AdviceItem<T2> //Use advice, value
{
    public bool UseAdvice { get; private set; }

    public T2 Second { get; private set; }

    internal AdviceItem(bool first, T2 second)
    {
        UseAdvice = first;
        Second = second;
    }
}

public static class AdviceItem
{
    public static AdviceItem<T2> New<T2>(bool first, T2 second)
    {
        var tuple = new AdviceItem<T2>(first, second);
        return tuple;
    }
}
