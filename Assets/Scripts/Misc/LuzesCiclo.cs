using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LuzesCiclo : MonoBehaviour
{

    Light2D luz2d;
    bool mudarCor;
    Color cor;
    float velocidadeMudanca;

    private void Start()
    {
        luz2d = GetComponent<Light2D>();
    }

    private void FixedUpdate()
    {
        if (mudarCor)
        {
            luz2d.color = Color.Lerp(luz2d.color, cor, velocidadeMudanca * Time.deltaTime);
            if (luz2d.color == cor)
                mudarCor = false;
        }
    }

    public void MudarCorAmbiente(Color corNova, float vel=0)
    {
        if(vel == 0)
            luz2d.color = corNova;
        else
        {
        mudarCor = true; cor = corNova; velocidadeMudanca = vel;
        }
    }
}
