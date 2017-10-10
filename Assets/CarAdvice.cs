using UnityEngine;
using UnityEditor;

public class CarAdvice
{
    public const int TURN_LEFT = -1;
    public const int TURN_RIGHT = 1;
    public const int TURN_NONE = 0;

    public const int ACCELERATE_FW = 1;
    public const int ACCELERATE_BW = -1;
    public const int BRAKE = 0;

    private AdviceItem<int> moveType;

    public AdviceItem<int> MoveType
    {
        get { return moveType; }
        set { moveType = value; }
    }
    
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
// Dit is een test comment niet naar kijken pls

public static class AdviceItem
{
    public static AdviceItem<T2> New<T2>(bool first, T2 second)
    {
        var tuple = new AdviceItem<T2>(first, second);
        return tuple;
    }
}
