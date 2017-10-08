using UnityEngine;
using System.Collections;

public class MovingObstacle : MonoBehaviour
{
    public Vector3[] endPositions = new Vector3[1];
    public bool shouldLoop = true;
    public bool resetPositionOnLoop = true;
    public bool useRelativePositions = true;

    private Vector3 startPosition;
    private int currentEndPosition = 0;
    private Vector3 currentEndVector;

    private bool finished = false;


    public float movementSpeed = .2f;
    // Use this for initialization
    void Start()
    {
        startPosition = gameObject.transform.position;

        if (useRelativePositions)
        {
            for (int i = 0; i < endPositions.Length; i++)
            {
                Vector3 previousStartPoint;
                if (i == 0)
                    previousStartPoint = startPosition;
                else
                    previousStartPoint = endPositions[i - 1];

                endPositions[i] += previousStartPoint;
            }
        }


        SetCurrentEndVector();
    }

    // Update is called once per frame
    void Update()
    {
        if (!finished)
            Move();
        if (gameObject.transform.position == currentEndVector)
        {
            currentEndPosition++;
            if (currentEndPosition >= endPositions.Length)
            {
                if (shouldLoop)
                {
                    currentEndPosition = 0;
                    if (resetPositionOnLoop)
                        gameObject.transform.position = startPosition;
                }
                else
                {
                    finished = true;
                }  
            }
            
            SetCurrentEndVector();
        }
    }

    void Move()
    {
            gameObject.transform.position =
                Vector3.MoveTowards(
                    gameObject.transform.position,
                    currentEndVector,
                    movementSpeed);
    }

    void SetCurrentEndVector()
    {
        if(currentEndPosition < endPositions.Length)
            currentEndVector = endPositions[currentEndPosition];
    }
}
