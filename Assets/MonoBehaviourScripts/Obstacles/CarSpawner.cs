using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour {
    // Use this for initialization
    public int carRouteID;
    public GameObject Freecar;
    private GameObject ClonedCar;
    private GameObject ClonedCarObject;
    public string prefab;
    private bool SpawnerToggle = false;
    void Start () {
        InvokeRepeating("Spawn", 2f, 3f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Spawn()
    {
        if (SpawnerToggle == true)
        {
            ClonedCar = (GameObject)Instantiate(Resources.Load(prefab));
            ClonedCar.transform.position = this.transform.position;
            ClonedCar.AddComponent<CarRoute>().CarRouteNumber = carRouteID;
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
