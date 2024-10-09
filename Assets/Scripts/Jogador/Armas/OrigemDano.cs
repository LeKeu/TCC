using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrigemDano : MonoBehaviour
{
    [SerializeField] private int valorDano = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        InimigoVida inimigoVida = collision.GetComponent<InimigoVida>();
        inimigoVida?.ReceberDano(valorDano);

        InvocadoInimigo invocadoInimigo = collision.GetComponent<InvocadoInimigo>();
        invocadoInimigo?.ReceberDano(valorDano); // dano no invocado do saci

        Saci saci = collision.GetComponent<Saci>();
        saci?.ReceberDano(valorDano);

        Iara iara = collision.GetComponent<Iara>();
        iara?.ReceberDano(valorDano);

        Cuca cuca = collision.GetComponent<Cuca>();
        cuca?.ReceberDano(valorDano);

        //HitStop.hitStop(1);
    }
}
