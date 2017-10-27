using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCheckpointCollisionScript : MonoBehaviour {
    private AutonomousDriveBehaviour autonomousDriveBehaviour;
    public AutonomousDriveBehaviour DriveBehaviour
    {
        get { return autonomousDriveBehaviour; }
        set { autonomousDriveBehaviour = value; }
    }

    private CarCheckpoint checkPointBox;
    public CarCheckpoint CheckPointBox
    {
        get { return checkPointBox; }
        set { checkPointBox = value; }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CheckPoint") // If the object it collides with is a checkpoint, the car will change the direction.
        {
            autonomousDriveBehaviour.GoToPosition = CheckPointBox.transform.position;
        }
    }
}

