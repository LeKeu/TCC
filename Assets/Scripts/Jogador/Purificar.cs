using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Purificar : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<InimigoVida>())
        {
            InimigoVida inimigoVida = collision.GetComponent<InimigoVida>();
            Debug.Log(inimigoVida.estaAtordoado);
            if (inimigoVida.estaAtordoado)
            { Debug.Log("RANGE!"); inimigoVida.EstaNaRange = true; }
        }   
    }

    
}
