using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//so that we can see changes we make without having to run the game

[ExecuteInEditMode]
[System.Serializable]
public class LidarDepthMapScript : MonoBehaviour
{
    public Material mat;
    // Use this for initialization
    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
        //mat is the material which contains the shader
        //we are passing the destination RenderTexture to

    }
}
