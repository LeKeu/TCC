using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LuzesCiclo : MonoBehaviour
{
    public float duration = 30f;

    [SerializeField] private Gradient gradient;
    [Range(0, 1)]
    [SerializeField] private float _cycleValue;
    private Light2D _light;
    private float _startTime;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
        _startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the time elapsed since the start time
        var timeElapsed = Time.time - _startTime;
        // Calculate the percentage based on the sine of the time elapsed
        var percentage = Mathf.Sin(timeElapsed / duration * Mathf.PI * 2) * 0.5f + 0.5f;
        Debug.Log("What is 0: " + percentage);
        // Clamp the percentage to be between 0 and 1
        percentage = Mathf.Clamp01(percentage);
        Debug.Log("What is 1: " + percentage);
        _light.color = gradient.Evaluate(_cycleValue);
    }
}
