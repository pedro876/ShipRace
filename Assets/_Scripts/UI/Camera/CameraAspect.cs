using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAspect : MonoBehaviour
{
    [SerializeField] CameraResolution camRes;
    Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        camRes.onTextureChanged += () =>
        {
            cam.aspect = (float)camRes.tex.width / (float)camRes.tex.height;
        };
    }
}
