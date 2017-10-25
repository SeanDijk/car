using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerBoxObstacle : MonoBehaviour {

    public int id;
    private bool isActive;
    private CarSpawnController controller;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "CheckpointCollisionChecker")
        {
            Debug.Log("Yee boi");
            controller.turnOnSpawner(id);
            isActive = true;
        }
    }
    void OnTriggerStay(Collider other)
    {

    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "CheckpointCollisionChecker")
        {
            isActive = false;
            controller.turnOffSpawner(id);
        }
    }

    public bool isTriggerActive()
    {
        return isActive;
    }

    public void setController(CarSpawnController GO)
    {
        controller = GO;
    }
}
