using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoVida : MonoBehaviour
{
    [SerializeField] private int vidaInicial = 3;
    [SerializeField] private float empurraoThrust = 15f;

    private int vidaAtual;
    private Empurrao empurrao;
    private Flash flash;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        empurrao = GetComponent<Empurrao>();
    }

    private void Start()
    {
        vidaAtual = vidaInicial;
    }

    public void ReceberDano(int dano)
    {
        vidaAtual -= dano;
        empurrao.SerEmpurrado(JogadorController.Instance.transform, empurraoThrust);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(ChecarMorteRotina());
    }

    private IEnumerator ChecarMorteRotina()
    {
        yield return new WaitForSeconds(flash.PegarTempoDeRestore());
        ChecarMorte();
    }

    public void ChecarMorte()
    {
        if(vidaAtual <= 0) { Destroy(gameObject); }
    }
}
