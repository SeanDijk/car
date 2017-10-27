using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : AbstractSensor<RadarSensorBehaviour> {
    
	// Use this for initialization
	void Start () {        
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void AttachListener(RadarSensorBehaviour listener)
    {
        myListener = listener;
    }

    //If the gameobject is a checkpoint, pass it on to the listener
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "CheckPoint" && other.gameObject.tag != "SpawnTrigger")
        {
            Debug.Log(other.gameObject.tag + "    " + other.gameObject.name);
            myListener.OnTriggerEnter(other);
        }
    }
    //If the gameobject is a checkpoint, pass it on to the listener
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name != "CheckPoint" && other.gameObject.tag != "SpawnTrigger")
        {
            myListener.OnTriggerExit(other);
        }
    }
}
