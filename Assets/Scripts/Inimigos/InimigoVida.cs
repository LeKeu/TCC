using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoVida : MonoBehaviour
{
    [SerializeField] private int vidaInicial = 3;

    private int vidaAtual;

    private void Start()
    {
        vidaAtual = vidaInicial;
    }

    public void ReceberDano(int dano)
    {
        vidaAtual -= dano;
        Debug.Log(vidaAtual);
        ChecarMorte();
    }

    private void ChecarMorte()
    {
        if(vidaAtual <= 0) { Destroy(gameObject); }
    }
}
