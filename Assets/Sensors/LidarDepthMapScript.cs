using System;
using System.IO;
using UnityEngine;

//using UnityEngine.VR.WSA.WebCam;
//so that we can see changes we make without having to run the game

[System.Serializable]
public class LidarDepthMapScript : MonoBehaviour
{
    public Material mat;

    private ScreenRecorder screenRecorder = null;
    private Camera myCam = null;

    private string folder = null;

    private void Start()
    {
        //Load up the screenrecorder
        screenRecorder = (ScreenRecorder)ScriptableObject.CreateInstance(typeof(ScreenRecorder));
        screenRecorder.captureWidth = 1280;
        screenRecorder.captureHeight = 720;

        folder = Path.GetFullPath(Application.dataPath + "/..");
        folder += "/lidar";
        System.IO.Directory.CreateDirectory(folder);

        ChangeFolder();

        //Load the depthtexture
        myCam = GetComponent<Camera>();
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;

        InvokeRepeating("LidarAction", 1f, 1f);
    }

    private void Update()
    {
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
        //mat is the material which contains the shader
        //we are passing the destination RenderTexture to
    }

    private void CaptureImage()
    {
        screenRecorder.CaptureScreenshot(myCam);
    }

    private int rotateCounter = 0;
    private int folderCounter = 0;

    // Rotates the camera 90 degrees to the right
    private void RotateCamera90Degrees()
    {
        transform.RotateAround(transform.position, transform.up, 90f);
        rotateCounter++;

        if (rotateCounter > 0 && rotateCounter % 4 == 0)
        {
            rotateCounter = 0;
            folderCounter++;
        }
        ChangeFolder();
    }

    // Changes the folder acording the the rotation.
    private void ChangeFolder()
    {
        var tempString = folder + "/" + folderCounter;

        switch (rotateCounter)
        {
            case 0: tempString += "/front"; break;
            case 1: tempString += "/right"; break;
            case 2: tempString += "/back"; break;
            case 3: tempString += "/left"; break;
        }

        System.IO.Directory.CreateDirectory(tempString);
        screenRecorder.folder = tempString;
    }

    private void LidarAction()
    {
        CaptureImage();
        RotateCamera90Degrees();
    }
}