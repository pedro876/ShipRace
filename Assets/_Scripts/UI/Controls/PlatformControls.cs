using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlatformControls : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI keyboardText;
    [SerializeField] TextMeshProUGUI gamepadText;
    [SerializeField] TextMeshProUGUI mobileText;
    [SerializeField] Button keyboardButton;
    [SerializeField] Button mobileButton;

    // Start is called before the first frame update
    void Awake()
    {
        keyboardText.gameObject.SetActive(false);
        gamepadText.gameObject.SetActive(false);
        mobileText.gameObject.SetActive(false);

#if UNITY_ANDROID
        DestroyImmediate(keyboardButton.gameObject);
        mobileText.gameObject.SetActive(true);
#else
        DestroyImmediate(mobileButton.gameObject);
        keyboardText.gameObject.SetActive(true);
#endif
    }
}
