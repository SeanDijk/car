using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "CarBoxCollider")
        {
            Debug.Log("Its the car box collider");
            
        }
        else
        {
            Debug.Log("Its something else");
        }
        Debug.Log("Name: " + other.gameObject.ToString());
    }
    void OnTriggerStay(Collider other)
    {

    }
    void OnTriggerExit(Collider other)
    {

    }
}
