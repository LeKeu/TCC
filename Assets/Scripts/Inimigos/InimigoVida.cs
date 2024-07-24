using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class InimigoVida : MonoBehaviour
{
    [SerializeField] private int vidaInicial = 3;
    [SerializeField] private float empurraoThrust = 15f;
    float TempoAtordoamento = 20f;

    private int vidaAtual;
    private Empurrao empurrao;
    private Flash flash;

    public bool estaAtordoado;
    public bool estaTempoAtordoamento;
    public bool EstaNaRange;
    float tempoAtual = 0f;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        empurrao = GetComponent<Empurrao>();
    }

    private void Start()
    {
        vidaAtual = vidaInicial;
    }

    private void Update()
    {
        if (estaAtordoado) 
        { 
            SerPurificado(tempoAtual);
            tempoAtual += Time.deltaTime;
        }
    }

    public void ReceberDano(int dano)
    {
        vidaAtual -= dano;
        //Debug.Log($"RECEBER DANO {dano}");
        empurrao.SerEmpurrado(JogadorController.Instance.transform, empurraoThrust);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(ChecarMorteRotina());
    }

    public void SerPurificado(float tempoAtual)
    {
        //tempoAtual = 0f;
        //estaAtordoado = true;

        if (tempoAtual <= TempoAtordoamento)
        {
            if (Keyboard.current.qKey.wasPressedThisFrame && EstaNaRange)
            {
                Debug.Log("purificadoww!");
                Destroy(gameObject);
                return;
            }
            //Debug.Log(tempoAtual);
            //SerPurificado(tempoAtual);
        }
        else
        {
            //Debug.Log("voltou a vida!");
            estaAtordoado = false;
            return;
        }
        
    }

    private IEnumerator ChecarMorteRotina()
    {
        yield return new WaitForSeconds(flash.PegarTempoDeRestore());
        ChecarMorte();
    }

    public void ChecarMorte()
    {
        //if(vidaAtual <= 0) { Destroy(gameObject); }
        if (vidaAtual <= 0) { estaAtordoado = true; }
    }
}
