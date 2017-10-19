﻿
using System;
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
    void Update()
    {
        var newNum = Convert.ToInt32(Time.fixedTime);
        if (newNum > prev)
        {
            prev = newNum;
            for (int i = 0; i < 4; i++)
            {
                ScreenCapture.CaptureScreenshot("test/" + prev + "_" + i+ ".png");
                transform.RotateAround(transform.position, transform.up, 90f);
            }

        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
        //mat is the material which contains the shader
        //we are passing the destination RenderTexture to

    }


}
