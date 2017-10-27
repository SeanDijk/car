using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class CarCheckpoint : MonoBehaviour {
    public List<Vector3> checkpointList = new List<Vector3>(); // List containing the vector3's of the checkpoint places
    public List<float> checkpointListRotation = new List<float>(); // List containing the rotayion of the checkpoint places
    private int checkpointCounter = 0;
    private string path;
    private string jsonString;
    private Vector3 newVector3;
    private JsonData checkpointData;
    private float current_x;
    private float current_y;
    private float current_z;
    private float current_rotation;
    int counter = 0;
    // Use this for initialization

    /*
     * The Awake method is used here instead of Start. Start runs on the first frame the script is active, awake on initialization.
     * Awake is needed so that an object using this script can be teleported in a trigger as soon as it is created.
     */
    void Awake () {
        path = Application.streamingAssetsPath + "/checkpointcoordinates.json"; // Path to the json file
        jsonString = File.ReadAllText(path);    // Reads json file
        checkpointData = JsonMapper.ToObject(jsonString); 
        for (int i = 0; i < checkpointData["checkpoints"].Count;) // loop trough all checkpoint object in the array 
        {
            current_x = float.Parse(checkpointData["checkpoints"][i]["x"].ToString());
            current_y = float.Parse(checkpointData["checkpoints"][i]["y"].ToString());
            current_z = float.Parse(checkpointData["checkpoints"][i]["z"].ToString());
            current_rotation = float.Parse(checkpointData["checkpoints"][i]["rotation"].ToString());
            newVector3 = new Vector3(current_x, current_y, current_z);
            checkpointList.Add(newVector3);
            checkpointListRotation.Add(current_rotation);
            i++;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "CarTriggerBox") // If there is a collision with the checkpointcollider of the car, it will change it's position
        {
            if (checkpointList.Count > checkpointCounter)
            {
                Debug.Log(checkpointCounter);
                this.transform.position = checkpointList[checkpointCounter];
                this.gameObject.transform.rotation = Quaternion.AngleAxis(checkpointListRotation[checkpointCounter], Vector3.up);
                checkpointCounter++;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
        }

    }
}

