using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerScript : MonoBehaviour {
    public List<Vector3> checkpointList = new List<Vector3>(); // List containing the vector3's of the checkpoint places
    private int checkpointCounter = 0;
    // Use this for initialization
    void Start () {
        // Adding the checkpoints to the list
        /*checkpointList.Add(new Vector3(-60f, -2.74f, -120f));
        checkpointList.Add(new Vector3(-40f, -2.74f, -150f));
        checkpointList.Add(new Vector3(-20f, -2.74f, -180f));
        checkpointList.Add(new Vector3(-5f, -2.74f, -220f));
        checkpointList.Add(new Vector3(20f, -2.74f, -260));
        checkpointList.Add(new Vector3(14f, -2.74f, -355));*/
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "CheckpointCollisionChecker")
        {
        }
        else
        {
            return;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "CheckpointCollisionChecker") // If there is a collision with the checkpointcollider of the car, it will change it's position
        {
            this.transform.position = checkpointList[checkpointCounter];
            checkpointCounter++;
      

        }
    }
    void OnTriggerExit(Collider other)
    {
    }
}
