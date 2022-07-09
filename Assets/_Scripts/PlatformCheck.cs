using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformCheck : MonoBehaviour
{
    [SerializeField] UnityEvent onAndroid;
    [SerializeField] UnityEvent onPC;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID || UNITY_EDITOR
        onAndroid?.Invoke();
#else
        onPC?.Invoke();
#endif
    }
}
