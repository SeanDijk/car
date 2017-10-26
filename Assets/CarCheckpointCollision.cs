using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCheckpointCollision : MonoBehaviour {
    private AutonomousDriveBehaviour autonomousDriveBehaviour;
    public AutonomousDriveBehaviour DriveBehaviour
    {
        get { return autonomousDriveBehaviour; }
        set { autonomousDriveBehaviour = value; }
    }

    private OnTriggerScript checkPointBox;
    public OnTriggerScript CheckPointBox
    {
        get { return checkPointBox; }
        set { checkPointBox = value; }
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {

    }
    void OnTriggerStay(Collider other)
    {

    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CheckPoint") // If the object it collides with is a checkpoint, the car will change the direction.
        {
            autonomousDriveBehaviour.GoToPosition = CheckPointBox.transform.position;
        }
    }
}

