
using System;
using System.IO;
using UnityEngine;
using UnityEngine.VR.WSA.WebCam;
//so that we can see changes we make without having to run the game

[ExecuteInEditMode]
[System.Serializable]
public class LidarDepthMapScript : MonoBehaviour
{
    PhotoCapture photoCaptureObject = null;
    Texture2D targetTexture = null;
    public Material mat;
    // Use this for initialization
    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;


    }

    // Update is called once per frame
    int prev = 0;
    int counter = 0;
    const int MAX_COUNTER = 3;
    void Update()
    {
        var newNum = Convert.ToInt32(Time.fixedTime);
        if (newNum > prev)
        {
            transform.RotateAround(transform.position, transform.up, 90f * MAX_COUNTER - counter);
            prev = newNum;
            counter = 0;
            Directory.CreateDirectory("test/" + newNum);
        }
        if(counter < MAX_COUNTER)
        {
            ScreenCapture.CaptureScreenshot("test/" + newNum + "/" + counter + ".png");
            transform.RotateAround(transform.position, transform.up, 90f);
            counter++;
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
        //mat is the material which contains the shader
        //we are passing the destination RenderTexture to

    }


}
