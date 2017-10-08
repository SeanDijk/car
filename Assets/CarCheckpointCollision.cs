using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCheckpointCollision : MonoBehaviour {
    public Car car;
    public OnTriggerScript checkpointBox;
    private int checkpointCounter = 1;
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
        if (other.gameObject.name == "CheckPoint") // If the object it collides with is a checkpoint, the car will change the direction.
        {
            car.ChangeDirection(checkpointBox.transform.position);
            checkpointCounter++;
        }
    }
}

