using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraResolution : MonoBehaviour
{
    Camera cam;
    [SerializeField] int height = 192;
    int width;
    [HideInInspector] public RenderTexture tex;

    public event Action onTextureChanged;

    private void Start()
    {
        cam = GetComponent<Camera>();
        CheckTexture();
    }

    private void Update()
    {
        CheckTexture();
    }

    private void CheckTexture()
    {
        width = (int)(((float)Screen.width / (float)Screen.height) * height);
        if(cam.targetTexture.width != width)
        {
            RenderTextureDescriptor descriptor = new RenderTextureDescriptor(width, height);
            tex = new RenderTexture(descriptor);
            tex.name = $"Tex {tex.width}x{tex.height}"; 
            tex.filterMode = FilterMode.Point;
            cam = GetComponent<Camera>();
            cam.targetTexture = tex;
            Debug.Log($"Final resolution: {width}x{height}");
            onTextureChanged?.Invoke();
        }
    }
}
