using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class SpawnScale
{
    float x { get; set; }
    float y { get; set; }
    float z { get; set; }
}

public class ObjectSpawnController : MonoBehaviour
{
    private string path;
    private string jsonString;
    private JsonData spawnData;
    private float current_spawn_x;
    private float current_spawn_y;
    private float current_spawn_z;
    private float current_trigger_x;
    private float current_trigger_y;
    private float current_trigger_z;
    private float current_trigger_scale_x;
    private float current_trigger_scale_y;
    private float current_trigger_scale_z;
    private float current_trigger_rotation;
    private int current_id;
    private string current_spawn_object_type;
    private Vector3 current_spawn_vector;
    private Vector3 current_trigger_vector;
    private Vector3 current_trigger_vector_scale;
    private string current_spawn_type;
    private float current_spawn_randommax;
    private float current_spawn_randommin;
    private float current_spawn_fixedrate;
    private List<Vector3> spawnLocations = new List<Vector3>();
    private List<int> spawnLocationRouteIds = new List<int>();
    private List<Vector3> spawnTriggerLocations = new List<Vector3>();
    private List<Vector3> spawnTriggerScale = new List<Vector3>();
    private List<float> spawnTriggerRotation = new List<float>();
    private List<string> spawnTriggerObject = new List<string>();
    private List<GameObject> gameObjectSpawnerList = new List<GameObject>();
    private List<GameObject> gameObjectTriggerList = new List<GameObject>();
    private List<string> spawnLocationSpawnType = new List<string>();
    private List<float[,]> spawnTimeOptions = new List<float[,]>();
    private GameObject spawnPoint;
    private GameObject triggerBox;
    // Use this for initialization
    void Start()
    {
        getSpawnLocation();
        createSpawnLocations();
        createTriggerLocations();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void turnOnSpawner(int i)
    {
        i = i - 1;
        GameObject spawnObject = gameObjectSpawnerList[i];
        spawnObject.GetComponent<ObstacleSpawner>().ToggleSpawner(true);
    }
    public void turnOffSpawner(int i)
    {
        i = i - 1;
        GameObject spawnObject = gameObjectSpawnerList[i];
        spawnObject.GetComponent<ObstacleSpawner>().ToggleSpawner(false);

    }
    void getSpawnLocation() // Gets the location of the car spawner
    {
        path = Application.streamingAssetsPath + "/spawndata.json"; // Path to the json file
        jsonString = File.ReadAllText(path);    // Reads json file
        spawnData = JsonMapper.ToObject(jsonString);
        for (int i = 0; i < spawnData["spawnpoint"].Count;) // loop trough all checkpoint object in the array 
        {

            current_spawn_x = float.Parse(spawnData["spawnpoint"][i]["spawnpointData"]["x"].ToString());
            current_spawn_y = float.Parse(spawnData["spawnpoint"][i]["spawnpointData"]["y"].ToString());
            current_spawn_z = float.Parse(spawnData["spawnpoint"][i]["spawnpointData"]["z"].ToString());
            current_spawn_type = spawnData["spawnpoint"][i]["spawnpointData"]["spawntype"].ToString();
            current_spawn_randommax = float.Parse(spawnData["spawnpoint"][i]["spawnpointData"]["randommax"].ToString());
            current_spawn_randommin = float.Parse(spawnData["spawnpoint"][i]["spawnpointData"]["randommin"].ToString());
            current_spawn_fixedrate = float.Parse(spawnData["spawnpoint"][i]["spawnpointData"]["fixedspawntime"].ToString());


            current_spawn_object_type = spawnData["spawnpoint"][i]["obstacle_type"].ToString();
            current_trigger_x = float.Parse(spawnData["spawnpoint"][i]["triggerboxData"]["x"].ToString());
            current_trigger_y = float.Parse(spawnData["spawnpoint"][i]["triggerboxData"]["y"].ToString());
            current_trigger_z = float.Parse(spawnData["spawnpoint"][i]["triggerboxData"]["z"].ToString());
            current_trigger_rotation = float.Parse(spawnData["spawnpoint"][i]["triggerboxData"]["rotation"].ToString());
            current_trigger_scale_x = float.Parse(spawnData["spawnpoint"][i]["triggerboxData"]["scale_x"].ToString());
            current_trigger_scale_y = float.Parse(spawnData["spawnpoint"][i]["triggerboxData"]["scale_y"].ToString());
            current_trigger_scale_z = float.Parse(spawnData["spawnpoint"][i]["triggerboxData"]["scale_z"].ToString());
            current_spawn_vector = new Vector3(current_spawn_x, current_spawn_y, current_spawn_z);
            current_trigger_vector = new Vector3(current_trigger_x, current_trigger_y, current_trigger_z);
            current_trigger_vector_scale = new Vector3(current_trigger_scale_x, current_trigger_scale_y, current_trigger_scale_z);
            current_id = int.Parse(spawnData["spawnpoint"][i]["id"].ToString());

            float[,] spawnOptions = new float[,] { { current_spawn_randommax, current_spawn_randommin, current_spawn_fixedrate } };

            spawnLocationSpawnType.Add(current_spawn_type);
            spawnLocations.Add(current_spawn_vector); // Adds the vector3 to the list
            spawnTriggerObject.Add(current_spawn_object_type);
            spawnTriggerRotation.Add(current_trigger_rotation);
            spawnTriggerLocations.Add(current_trigger_vector);
            spawnLocationRouteIds.Add(current_id); // Adds the vector
            spawnTimeOptions.Add(spawnOptions);
            float test = current_trigger_scale_x;
            spawnTriggerScale.Add(current_trigger_vector_scale);
            Debug.Log("nr :" + i + ", x: " + current_spawn_x);
            i++;

        }
        Debug.Log(spawnLocations[1]);


    }

    void createSpawnLocations()
    {
        for (int i = 0; i < spawnLocations.Count;)
        {
            float spawnRate;
            spawnRate = determineRespawnRate(i, spawnLocationSpawnType[i]);
            spawnPoint = new GameObject();
            spawnPoint.AddComponent<ObstacleSpawner>().obstacleRouteID = spawnLocationRouteIds[i];
            spawnPoint.GetComponent<ObstacleSpawner>().setSpawnRate(spawnRate);
            spawnPoint.GetComponent<ObstacleSpawner>().setTypeObject(spawnTriggerObject[i]);
            
            Debug.Log("This is a test of TriggerObjects:" + spawnTriggerObject[i]);
            spawnPoint.transform.position = spawnLocations[i];
            spawnPoint.gameObject.name = "Spawnpoint " + (i + 1);
            gameObjectSpawnerList.Add(spawnPoint);
            i++;
        }
    }
    void createTriggerLocations()
    {
        for (int i = 0; i < spawnTriggerLocations.Count;)
        {
            triggerBox = GameObject.CreatePrimitive(PrimitiveType.Cube);

            triggerBox.AddComponent<triggerBoxObstacle>().id = spawnLocationRouteIds[i];
            triggerBox.transform.position = spawnTriggerLocations[i];
            triggerBox.transform.localScale = spawnTriggerScale[i];
            triggerBox.transform.rotation = Quaternion.AngleAxis(spawnTriggerRotation[i], Vector3.up);
            triggerBox.GetComponent<Collider>().isTrigger = true;
            triggerBox.gameObject.name = "TriggerSpawnpoint " + (i + 1); 
            triggerBox.GetComponent<MeshRenderer>().enabled = false;
            triggerBox.GetComponent<triggerBoxObstacle>().setController(this);

            gameObjectTriggerList.Add(triggerBox);
            i++;
        }
    }

    float determineRespawnRate(int i, string respawntype)
    {
        float maxRandom;
        float minRandom;
        float[,] combinedOptions = spawnTimeOptions[i];
        if (respawntype == "random")
        {
            maxRandom = combinedOptions[0, 0];
            minRandom = combinedOptions[0, 1];
            Debug.Log("It is Random");
            return Random.Range(minRandom, maxRandom);
        }
        else if (respawntype == "fixed")
        {
            Debug.Log("It is Fixed");
            return combinedOptions[0, 2];
        }
        else
        {
            return 0;
        }
    }
}
