using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class OnTriggerScript : MonoBehaviour {
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
    void Start () {
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
        // Just for testing purpose DELETE after 612.9 -739
    }

    // Update is called once per frame
    void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "CheckpointCollisionChecker") // If there is a collision with the checkpointcollider of the car, it will change it's position
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
    void OnTriggerStay(Collider other)
    {

    }
    void OnTriggerExit(Collider other)
    {
    }

}

