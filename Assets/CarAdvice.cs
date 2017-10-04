using UnityEngine;
using UnityEditor;

public class CarAdvice
{
    public readonly static int TURN_LEFT = -1;
    public readonly static int TURN_RIGHT = 1;
    public readonly static int TURN_NONE = 0;

    private bool shouldAccelerate;

    public bool ShouldAccelerate
    {
        get { return shouldAccelerate; }
        set { shouldAccelerate = value; }
    }
    private bool shouldReverse;

    public bool ShouldReverse
    {
        get { return shouldReverse; }
        set { shouldReverse = value; }
    }

    private bool shouldBrake;

    public bool ShouldBrake
    {
        get { return shouldBrake; }
        set { shouldBrake = value; }
    }

    private int turnDirection;

    public int TurnDirection
    {
        get { return turnDirection; }
        set { turnDirection = value; }
    }

    public CarAdvice(bool shouldAccelerate, bool shouldBrake, bool shouldReverse, int turnDirection)
    {
        ShouldAccelerate = shouldAccelerate;
        ShouldReverse = shouldReverse;
        ShouldBrake = shouldBrake;
        TurnDirection = turnDirection;
    }



}

public class Tuple<Boolean, T2>
{
    public bool UseAdvice { get; private set; }

    public T2 Second { get; private set; }

    internal Tuple(bool first, T2 second)
    {
        UseAdvice = first;
        Second = second;
    }
}


/**


public static class Tuple
{
    public static Tuple<T1, T2> New<T1, T2>(T1 first, T2 second)
    {
        var tuple = new Tuple<T1, T2>(first, second);
        return tuple;
    }
}
*/