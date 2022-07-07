using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFloat
{
    AnimationCurve curve;
    public float Value { get; private set; } = 0f;
    float maxChangeTime;

    float initialValue = 0f;
    float targetValue = 0f;
    float timer = 0f;
    float changeTime;
    public bool changing { get; private set; } = false;

    public SmoothFloat(float value = 0f, float maxChangeTime = 0.5f, AnimationCurve curve = null)
    {
        this.maxChangeTime = maxChangeTime;
        Value = value;
        if (curve != null)
            this.curve = curve;
        else
            this.curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    }

    public void SetValue(float newValue)
    {
        initialValue = Value;
        targetValue = newValue;
        if (targetValue != initialValue)
        {
            changeTime = Mathf.Abs(initialValue - targetValue)
                * maxChangeTime;
            timer = 0f;
            changing = true;
        }
    }

    public void SetCurve(AnimationCurve curve)
    {
        this.curve = curve;
    }

    public void SetMaxTime(float maxTime)
    {
        maxChangeTime = maxTime;
    }

    public void SetValueImmediate(float value)
    {
        Value = value;
    }

    public bool Update(float deltaTime)
    {
        if (changing)
        {
            timer = timer + deltaTime;
            if (timer >= changeTime)
            {
                changing = false;
                timer = changeTime;
            }
            float pointInCurve = curve.Evaluate(timer / changeTime);
            Value = Mathf.Lerp(initialValue, targetValue, pointInCurve);
            return true;
        }
        else
            return false;
    }
}
