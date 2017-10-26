using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class ObstacleRoute : MonoBehaviour
{
    public int ObstacleRouteNumber; // Determines what route should take.
    public List<Vector3> ObstacleRouteList = new List<Vector3>();
    private List<float> routeListRotation = new List<float>();

    private float current_x;
    private float current_y;
    private float current_z;
    private float current_rotation;
    private string pathRoute;
    private string obstacleType;
    private string pathObstacleInfo;
    private string jsonString;
    private JsonData routeData;
    private JsonData objectData;
    private float speed;
    private int currentPoint;
    private int i;
    private Vector3 nextDestination;
    private float nextRotation;
    private Quaternion targetRotation;
    private string jsonObstacleRouteString;
    // Use this for initialization
    void Start()
    {
        getRouteData();
        getObstacleProperties();
        Debug.Log(objectData[obstacleType].Count);
        Debug.Log(objectData[obstacleType]["speed"].ToString());
    }


    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, nextDestination, step);
        if (this.transform.position == nextDestination && currentPoint <= ObstacleRouteList.Count)
        {
            currentPoint++;

            if (currentPoint - 1 < ObstacleRouteList.Count)
            {
                try
                {
                    nextDestination = ObstacleRouteList[currentPoint];
                    transform.position = Vector3.MoveTowards(transform.position, nextDestination, step);
                    targetRotation = Quaternion.AngleAxis(routeListRotation[currentPoint], Vector3.up);
                }
                catch
                {
                    return;
                }
            }

            if (currentPoint - 1 == ObstacleRouteList.Count)
            {
                if (this.transform.position == nextDestination)
                {
                    Destroy(this.gameObject);
                }
            }
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        if (currentPoint == ObstacleRouteList.Count)
        {
            Destroy(this);
        }
    }

    void setCarRouteNumber(int routeNumber)
    {
        ObstacleRouteNumber = routeNumber;
    }
    private void getRouteData()
    {
        jsonObstacleRouteString = "route" + ObstacleRouteNumber.ToString();
        pathRoute = Application.streamingAssetsPath + "/obstacleroutes.json"; // Path to the json file
        jsonString = File.ReadAllText(pathRoute);    // Reads json file
        routeData = JsonMapper.ToObject(jsonString);
        for (i = 0; i < routeData[jsonObstacleRouteString][0].Count;) // loop trough all checkpoint object in the array 
        {
            current_x = float.Parse(routeData[jsonObstacleRouteString][0][i]["x"].ToString());
            current_y = float.Parse(routeData[jsonObstacleRouteString][0][i]["y"].ToString());
            current_z = float.Parse(routeData[jsonObstacleRouteString][0][i]["z"].ToString());
            current_rotation = float.Parse(routeData[jsonObstacleRouteString][0][i]["rotation"].ToString());
            Vector3 currentVector = new Vector3(current_x, current_y, current_z);
            ObstacleRouteList.Add(currentVector);
            routeListRotation.Add(current_rotation);
            i++;
        }

        nextDestination = ObstacleRouteList[0];
        nextRotation = routeListRotation[0];
        targetRotation = Quaternion.AngleAxis(nextRotation, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
    }
    private void getObstacleProperties()
    {
        pathObstacleInfo = Application.streamingAssetsPath + "/obstacleobjects.json"; // Path to the json file
        jsonString = File.ReadAllText(pathObstacleInfo);    // Reads json file
        objectData = JsonMapper.ToObject(jsonString);
        speed = float.Parse(objectData[obstacleType]["speed"].ToString());
    }

    public void setObstacleType(string name)
    {
        obstacleType = name;
    }
}
