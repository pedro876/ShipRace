using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] bool keepUpdating = true;

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!keepUpdating) return;
        UpdateText();
    }

    private void OnEnable()
    {
        UpdateText();
    }

    void UpdateText()
    {
        text.text = $"SCORE: {GameManager.instance.GetCurrentScore()}";
    }
}
