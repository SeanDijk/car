using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class ObstacleSpawner : MonoBehaviour {
    // Use this for initialization
    public int obstacleRouteID;
    public GameObject Freecar;
    private GameObject ClonedObstacle;
    public string objectName;
    private string prefab;
    private string pathRoute;
    private string jsonString;
    private float randommax;
    private float randommin;
    private JsonData objectData;
    private string pathObstacleInfo;
    private bool SpawnerToggle = false;
    private float respawnrate;
    private string spawntype;
    void Start () {
        getJson();
        Debug.Log("Respawn Rate: " + respawnrate);
        InvokeRepeating("Spawn", 0f, respawnrate);
        
    }
	
    void getJson(){
        pathObstacleInfo = Application.streamingAssetsPath + "/obstacleobjects.json";
        jsonString = File.ReadAllText(pathObstacleInfo);    // Reads json file
        objectData = JsonMapper.ToObject(jsonString);
        prefab = objectData[objectName]["prefabname"].ToString();
    }
	// Update is called once per frame
	void Update () {
		
	}

    void Spawn()
    {
        if (SpawnerToggle == true)
        {
            ClonedObstacle = (GameObject)Instantiate(Resources.Load(prefab));
            ClonedObstacle.transform.position = this.transform.position;
            ClonedObstacle.AddComponent<ObstacleRoute>().ObstacleRouteNumber = obstacleRouteID;
            ClonedObstacle.GetComponent<ObstacleRoute>().setObstacleType(objectName);
        }
        else
        {
            return;
        }
    }

    public void setSpawnRate(float rate)
    {
        respawnrate = rate;
    }

    public void ToggleSpawner(bool toggle)
    {
        SpawnerToggle = toggle;
    }

    public void setTypeObject(string name)
    {
        objectName = name;
    }
}
