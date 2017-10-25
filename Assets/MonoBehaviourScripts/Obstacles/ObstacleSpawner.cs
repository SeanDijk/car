using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {
    // Use this for initialization
    public int obstacleRouteID;
    public GameObject Freecar;
    private GameObject ClonedObstacle;
    public string prefab;
    private bool SpawnerToggle = false;
    private float randomFloat;
    void Start () {
        randomFloat = Random.Range(2f, 7f);
        Debug.Log(randomFloat);
        InvokeRepeating("Spawn", 2f, randomFloat);
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
        }
        else
        {
            return;
        }
    }

    public void ToggleSpawner(bool toggle)
    {
        SpawnerToggle = toggle;
    }
}
