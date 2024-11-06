using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LuzesCiclo : MonoBehaviour
{
    //public float duration = 30f;

    //[SerializeField] private Gradient gradient;
    //[Range(0, 1)]
    //[SerializeField] private float _cycleValue;
    //private Light2D _light;
    //private float _startTime;

    Light2D luz2d;
    bool mudarCor;
    Color cor;
    float velocidadeMudanca;

    //private void Awake()
    //{
    //    _light = GetComponent<Light2D>();
    //    _startTime = Time.time;
    //}

    private void Start()
    {
        luz2d = GetComponent<Light2D>();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    // Calculate the time elapsed since the start time
    //    var timeElapsed = Time.time - _startTime;
    //    // Calculate the percentage based on the sine of the time elapsed
    //    var percentage = Mathf.Sin(timeElapsed / duration * Mathf.PI * 2) * 0.5f + 0.5f;
    //    Debug.Log("What is 0: " + percentage);
    //    // Clamp the percentage to be between 0 and 1
    //    percentage = Mathf.Clamp01(percentage);
    //    Debug.Log("What is 1: " + percentage);
    //    _light.color = gradient.Evaluate(_cycleValue);
    //}

    private void FixedUpdate()
    {
        if (mudarCor)
        {
            Debug.Log("mudando cor");
            luz2d.color = Color.Lerp(luz2d.color, cor, 1f * Time.deltaTime);
            if (luz2d.color == cor)
                mudarCor = false;
        }
    }

    public void MudarCorAmbiente(Color corNova, float vel)
    {
        //luz2d.color = color;
        mudarCor = true; cor = corNova; velocidadeMudanca = vel;
    }
}
