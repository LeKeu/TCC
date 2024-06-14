using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrigemDano : MonoBehaviour
{
    [SerializeField] private int valorDano = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<InimigoIA>())
        {
            InimigoVida inimigoVida = collision.gameObject.GetComponent<InimigoVida>();
            inimigoVida.ReceberDano(valorDano);
        }
    }
}
