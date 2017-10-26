using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : AbstractSensor {
    private RadarSensorBehaviour myListener;
    
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
        if (other.gameObject.tag != "CheckPoint" )
        {
            myListener.OnTriggerEnter(other);
        }
    }
    //If the gameobject is a checkpoint, pass it on to the listener
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name != "CheckPoint")
        {
            myListener.OnTriggerExit(other);
        }
    }
}
