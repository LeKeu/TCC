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
            if (inimigoVida.estaAtordoado)
            { inimigoVida.EstaNaRange = true; }
        }   
    }

    
}
