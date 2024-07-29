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
    TremerCamera tremerCamera;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        empurrao = GetComponent<Empurrao>();
        tremerCamera = GameObject.Find("Virtual Camera").GetComponent<TremerCamera>();
    }

    void Start()
    {
        vidaAtual = vidaMax;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        InimigoIA inimigo = collision.gameObject.GetComponent<InimigoIA>();
        InimigoVida inimigoVida = collision.gameObject.GetComponent<InimigoVida>();

        if (inimigo && podeLevarDano && !inimigoVida.estaAtordoado)
        {
            LevarDano(1);
            empurrao.SerEmpurrado(collision.gameObject.transform, empurraoValor);
            StartCoroutine(flash.FlashRoutine());
        }
    }

    private void LevarDano(int dano)
    {
        if (!podeLevarDano) { return; }

        tremerCamera.TremerCameraFunc();

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
