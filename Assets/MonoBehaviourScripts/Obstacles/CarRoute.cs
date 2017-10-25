using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class CarRoute : MonoBehaviour {
    public int CarRouteNumber; // Determines what route should take.
    public List<Vector3> CarRouteList = new List<Vector3>();
    private List<float> routeListRotation = new List<float>();

    private float current_x;
    private float current_y;
    private float current_z;
    private float current_rotation;
    private string path;
    private string jsonString;
    private JsonData routeData;
    private float speed = 10;
    private int currentPoint;
    private int i;
    private Vector3 nextDestination;
    private float nextRotation;
    private Quaternion targetRotation;
    private string jsonCarRouteString;
    // Use this for initialization
    void Start () {
        jsonCarRouteString = "route" + CarRouteNumber.ToString();
        path = Application.streamingAssetsPath + "/obstacleroutes.json"; // Path to the json file
        jsonString = File.ReadAllText(path);    // Reads json file
        routeData = JsonMapper.ToObject(jsonString);
        for (i = 0; i < routeData[jsonCarRouteString][0].Count;) // loop trough all checkpoint object in the array 
        {
            current_x = float.Parse(routeData[jsonCarRouteString][0][i]["x"].ToString());
            current_y = float.Parse(routeData[jsonCarRouteString][0][i]["y"].ToString());
            current_z = float.Parse(routeData[jsonCarRouteString][0][i]["z"].ToString());
            current_rotation = float.Parse(routeData[jsonCarRouteString][0][i]["rotation"].ToString());
            Vector3 currentVector = new Vector3(current_x, current_y, current_z);
            CarRouteList.Add(currentVector);
            routeListRotation.Add(current_rotation);
            i++;
        }
        
        nextDestination = CarRouteList[0];
        nextRotation = routeListRotation[0];
    }


    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, nextDestination, step);
        if (this.transform.position == nextDestination && currentPoint <= CarRouteList.Count)
        {
            currentPoint++;

            if (currentPoint - 1 < CarRouteList.Count)
            {
                try
                {
                    nextDestination = CarRouteList[currentPoint];
                    transform.position = Vector3.MoveTowards(transform.position, nextDestination, step);
                    targetRotation = Quaternion.AngleAxis(routeListRotation[currentPoint], Vector3.up);
                }
                catch
                {
                    return;
                }
            }
            
            if (currentPoint - 1 == CarRouteList.Count)
            {
                if (this.transform.position == nextDestination)
                {
                    Destroy(this.gameObject);
                }
            }
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,  10f * Time.deltaTime);
        if (currentPoint == CarRouteList.Count)
        {
            Destroy(this);
        }
    }

    void setCarRouteNumber(int routeNumber)
    {
        CarRouteNumber = routeNumber;
    }
}
