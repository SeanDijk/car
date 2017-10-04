using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : AbstractSensor {
    public RadarSensorBehaviour myListener;
    private Collider myCollider;
    
	// Use this for initialization
	void Start () {
        myCollider = GetComponent<Collider>();
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnTriggerEnter(Collider other)
    {
        myListener.OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        myListener.OnTriggerExit(other);
    }
}
