using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
[ExecuteInEditMode]
public class TextStyle : MonoBehaviour
{
    [SerializeField] TextStylePreset stylePreset;
    [SerializeField] bool autoUpdate = true;
    [SerializeField] bool differentFontSize = false;

#if UNITY_EDITOR
    void Start()
    {
        ApplyStyle();
    }


    private void Update()
    {
        if (autoUpdate && !Application.isPlaying)
        {
            ApplyStyle();
        }
    }

    private void ApplyStyle()
    {
        if (stylePreset == null) return;

        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.font = stylePreset.fontAsset;
        text.color = stylePreset.vertexColor;
        text.enableVertexGradient = stylePreset.colorGradient;
        text.colorGradientPreset = stylePreset.colorGradientPreset;
        if(!differentFontSize)
            text.fontSize = stylePreset.fontSize;
    }
#endif
}
