using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    Camera cam;
    [SerializeField] int height = 224;
    int width;
    [HideInInspector] public RenderTexture tex;
    // Start is called before the first frame update
    void Start()
    {
        width = (int)(((float)Screen.currentResolution.width / (float)Screen.currentResolution.height) * height);
        RenderTextureDescriptor descriptor = new RenderTextureDescriptor(width, height);
        tex = new RenderTexture(descriptor);
        tex.filterMode = FilterMode.Point;
        cam = GetComponent<Camera>();
        cam.targetTexture = tex;
    }
}
