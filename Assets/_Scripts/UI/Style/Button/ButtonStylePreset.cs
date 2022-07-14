using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[CreateAssetMenu(menuName = "ConfigurationFiles/ButtonStylePreset")]
public class ButtonStylePreset : ScriptableObject
{
    public enum ImageFillOrigin
    {
        Bottom,
        Right,
        Top,
        Left
    }

    [Header("Image")]
    public Sprite sourceImage = null;
    public Color color = Color.white;
    public Material mat = null;
    public Image.Type imageType = Image.Type.Sliced;
    public bool preserveAspect = false;

    [Header("Simple Image properties")]
    public bool useSpriteMesh = false;

    [Header("Sliced or Tiled Image Properties")]
    public bool fillCenter = true;
    public float pixelsPerUnitMultiplier = 1f;

    [Header("Filled Image Properties")]
    public Image.FillMethod fillMethod = Image.FillMethod.Radial360;
    [SerializeField] ImageFillOrigin _fillOrigin = ImageFillOrigin.Bottom;
    public int fillOrigin => (int)_fillOrigin;
    [Range(0, 1)] public float fillAmount = 1f;
    public bool clockWise = true;

    [Header("Button")]
    public Button.Transition transition;

    [Header("Color Tint Transition")]
    public ColorBlock buttonColors = ColorBlock.defaultColorBlock;

    [Header("Sprite Swap Transition")]
    public SpriteState spriteState;

    [Header("Animation Transition")]
    public AnimationTriggers animationTriggers;

    [Header("Audio")]
    public AudioClip pressClip;
}
