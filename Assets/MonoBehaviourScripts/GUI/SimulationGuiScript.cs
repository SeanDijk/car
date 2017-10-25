using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationGuiScript : MonoBehaviour {
    private float speed;

    public float Speed
    {
        get { return speed; }
        set {
            speed = value;
            speedText.text = Mathf.FloorToInt(speed)+ " km/h";
        }
    }

    private int amountOfObjectsInFront;

    public int AmountOFObjectsInFront
    {
        get { return amountOfObjectsInFront; }
        set {
            amountOfObjectsInFront = value;
            amountOfObjectsInFrontText.text = value + " objects";
        }
    }



    Text speedText;
    Text amountOfObjectsInFrontText;


	// Use this for initialization
	void Start () {
        speedText = GameObject.Find("SpeedText").GetComponent<Text>();
        amountOfObjectsInFrontText = GameObject.Find("ObjectInFront").GetComponent<Text>();

    }

    // Update is called once per frame
    void Update () {
		
	}
}
