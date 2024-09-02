using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrigemDano : MonoBehaviour
{
    [SerializeField] private int valorDano = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        InimigoVida inimigoVida = collision.GetComponent<InimigoVida>();
        InvocadoInimigo invocadoInimigo = collision.GetComponent<InvocadoInimigo>();
        inimigoVida?.ReceberDano(valorDano);
        invocadoInimigo?.ReceberDano();
    }
}
