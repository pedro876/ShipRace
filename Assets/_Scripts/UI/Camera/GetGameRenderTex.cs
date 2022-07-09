using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetGameRenderTex : MonoBehaviour
{
    RawImage img;
    [SerializeField] CameraResolution cam;

    // Start is called before the first frame update
    void Awake()
    {
        img = GetComponent<RawImage>();
        img.color = Color.white;
        img.enabled = true;
        cam.onTextureChanged += () =>
        {
            img.texture = cam.tex;
        };
    }
}
