using System.Collections;

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.VR.WSA.WebCam;


[ExecuteInEditMode]

public class LidarCam : MonoBehaviour
{



    public Material material;
    public float Fov;
    public int SupersampleScale;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetFloat("_Fov", Fov);
        material.SetFloat("_SupersampleScale", SupersampleScale);
        Graphics.Blit(source, destination, material);
    }


   
}