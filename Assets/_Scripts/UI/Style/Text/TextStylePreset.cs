using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "ConfigurationFiles/TextStylePreset")]
public class TextStylePreset : ScriptableObject
{
    public TMP_FontAsset fontAsset;
    public Color vertexColor = Color.white;
    public bool colorGradient = false;
    public TMP_ColorGradient colorGradientPreset = null;
    public float fontSize = 12f;
}
