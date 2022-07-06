using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class ButtonStyle : MonoBehaviour
{
    [SerializeField] ButtonStylePreset stylePreset;

    // Start is called before the first frame update
    void Start()
    {
        ApplyStyle();
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    private void Update()
    {
        Debug.Log("Applying style");
        ApplyStyle();
    }
#endif

    void ApplyStyle()
    {
        if (stylePreset != null)
        {
            Button btn = GetComponent<Button>();
            Image img = GetComponent<Image>();

            //IMAGE
            img.sprite = stylePreset.sourceImage;
            img.color = stylePreset.color;
            img.material = stylePreset.mat;
            img.type = stylePreset.imageType;
            img.preserveAspect = stylePreset.preserveAspect;

            //simple
            img.useSpriteMesh = stylePreset.useSpriteMesh;

            //sliced and tiled
            img.fillCenter = stylePreset.fillCenter;
            img.pixelsPerUnitMultiplier = stylePreset.pixelsPerUnitMultiplier;

            //filled
            img.fillMethod = stylePreset.fillMethod;
            img.fillOrigin = stylePreset.fillOrigin;
            img.fillAmount = stylePreset.fillAmount;
            img.fillClockwise = stylePreset.clockWise;

            //BUTTON
            btn.transition = stylePreset.transition;
            btn.colors = stylePreset.buttonColors;
            btn.spriteState = stylePreset.spriteState;
            btn.animationTriggers = stylePreset.animationTriggers;
        }
    }
}
