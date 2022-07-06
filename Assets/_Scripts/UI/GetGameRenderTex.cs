using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetGameRenderTex : MonoBehaviour
{
    RawImage img;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<RawImage>();
        RenderTexture tex = Camera.main.targetTexture;
        img.texture = tex;
        img.color = Color.white;
        img.enabled = true;
    }
}
