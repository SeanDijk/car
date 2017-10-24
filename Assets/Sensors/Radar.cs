using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : AbstractSensor {
    public RadarSensorBehaviour myListener;
    private Collider myCollider;
    
	// Use this for initialization
	void Start () {        
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "CheckPoint" || other.gameObject.name != "Roads")
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
