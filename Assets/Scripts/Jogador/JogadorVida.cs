using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JogadorVida : MonoBehaviour
{
    [SerializeField] private int vidaMax = 3;
    [SerializeField] private float empurraoValor = 10f;
    [SerializeField] private float tempoRecoveryDano = 1f;

    private int vidaAtual;
    private bool podeLevarDano = true;
    private Empurrao empurrao;
    private Flash flash;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        empurrao = GetComponent<Empurrao>();
    }

    void Start()
    {
        vidaAtual = vidaMax;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        InimigoIA inimigo = collision.gameObject.GetComponent<InimigoIA>();

        if (inimigo && podeLevarDano)
        {
            LevarDano(1);
            empurrao.SerEmpurrado(collision.gameObject.transform, empurraoValor);
            StartCoroutine(flash.FlashRoutine());
        }
    }

    private void LevarDano(int dano)
    {
        if (!podeLevarDano) { return; }

        //TremerTela.Instance.FuncTremerTela();
        podeLevarDano = false;
        vidaAtual -= dano;
        StartCoroutine(RecoveryDanoRoutine());
    }

    private IEnumerator RecoveryDanoRoutine()
    {
        yield return new WaitForSeconds(tempoRecoveryDano);
        podeLevarDano = true;
    }
}
