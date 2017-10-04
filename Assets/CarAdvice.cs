using UnityEngine;
using UnityEditor;

public class CarAdvice
{
    public readonly static int TURN_LEFT = -1;
    public readonly static int TURN_RIGHT = 1;
    public readonly static int TURN_NONE = 0;

    private AdviceItem<bool> shouldAccelerate;

    public AdviceItem<bool> ShouldAccelerate
    {
        get { return shouldAccelerate; }
        set { shouldAccelerate = value; }
    }
    private AdviceItem<bool> shouldReverse;

    public AdviceItem<bool> ShouldReverse
    {
        get { return shouldReverse; }
        set { shouldReverse = value; }
    }

    private AdviceItem<bool> shouldBrake;

    public AdviceItem<bool> ShouldBrake
    {
        get { return shouldBrake; }
        set { shouldBrake = value; }
    }

    private AdviceItem<int> turnDirection;

    public AdviceItem<int> TurnDirection
    {
        get { return turnDirection; }
        set { turnDirection = value; }
    }

    public CarAdvice(AdviceItem<bool> shouldAccelerate, AdviceItem<bool> shouldBrake, AdviceItem<bool> shouldReverse, AdviceItem<int> turnDirection)
    {
        ShouldAccelerate = shouldAccelerate;
        ShouldReverse = shouldReverse;
        ShouldBrake = shouldBrake;
        TurnDirection = turnDirection;
    }

    public CarAdvice(bool shouldAccelerate,bool shouldBrake, bool shouldReverse, int turnDirection)
    {
        ShouldAccelerate = new AdviceItem<bool>(false, shouldAccelerate);
        ShouldReverse = new AdviceItem<bool>(false, shouldReverse);
        ShouldBrake = new AdviceItem<bool>(false, shouldBrake);
        TurnDirection = new AdviceItem<int>(false, turnDirection);
    }

    public void Combine(CarAdvice other)
    {
        if (other.ShouldAccelerate.UseAdvice)
            ShouldAccelerate = other.ShouldAccelerate;
        if (other.ShouldBrake.UseAdvice)
            ShouldBrake = other.ShouldBrake;
        if (other.ShouldReverse.UseAdvice)
            ShouldReverse = other.ShouldReverse;
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
