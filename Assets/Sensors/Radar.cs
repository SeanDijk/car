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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "CheckPoint" )
        {
            Debug.Log("This fucker " + other.gameObject.name);
            myListener.OnTriggerEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.name != "CheckPoint")
        {
            myListener.OnTriggerExit(other);
        }
    }
}
