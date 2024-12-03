using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class InimigoVida : MonoBehaviour
{
    [SerializeField] private int vidaInicial = 3;
    [SerializeField] private float empurraoThrust = 15f;
    float TempoAtordoamento = 4f;

    private int vidaAtual;
    private Empurrao empurrao;
    private Flash flash;
    InimigoIA inimigoIA;

    public bool estaAtordoado;
    public bool EstaNaRange;
    float tempoAtual = 0f;

    Rigidbody2D rb;
    Animator animator;

    public bool estaCorrompido;
    bool dropouObj; // dropar apenas uma vez o obj qnd for purificado

    private void Awake()
    {
        estaCorrompido = true;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        flash = GetComponent<Flash>();
        empurrao = GetComponent<Empurrao>();
        inimigoIA = GetComponent<InimigoIA>();
    }

    private void Start()
    {
        vidaAtual = vidaInicial;
    }

    private void Update()
    {
        if (estaAtordoado) 
        {
            animator.SetBool("atordoado", true);

            SerPurificado();
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

    public void SerPurificado()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (tempoAtual <= TempoAtordoamento)
        {
            if (Keyboard.current.qKey.wasPressedThisFrame && EstaNaRange) // está atordoado e vamos chegar perto para purificar
            {
                inimigoIA.VirarPurificado();
                if (!dropouObj) { gameObject.GetComponent<PegarExtras>().DroparItens(); dropouObj = true; }
                    
                //Destroy(gameObject);
                return;
            }
        }else if (tempoAtual > TempoAtordoamento)
        {
            EstaNaRange = false; estaAtordoado = false;
            animator.SetBool("atordoado", false);
            rb.constraints = RigidbodyConstraints2D.None; rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            vidaAtual = vidaInicial;
            tempoAtual = 0f;

            return;
        }
    }

    private IEnumerator ChecarMorteRotina()
    {
        yield return new WaitForSeconds(flash.PegarTempoDeRestore());
        if (vidaAtual <= 0) { estaAtordoado = true; }
    }
}
