using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InvocadoInimigo : BasicAndar
{
    TremerCamera tremerCamera;

    [SerializeField] public float movVel = 2f;
    [SerializeField] public float pegarDist = 2f;
    [SerializeField] public int Vida = 4;
    [SerializeField] public int Dano = 2;
    float velInicial;

    //Rigidbody2D rb;
    Vector2 movDir;
    private Empurrao empurrao;
    float empurraoForca = 10;
    SpriteRenderer spriteRenderer;
    CircleCollider2D circleCollider;

    public static bool podeAndar;
    public static bool podePurificar;

    bool estaPurificado;
    bool estaAtordoado;
    bool estaNaRange;

    private void Awake()
    {
        velInicial = movVel;
        podePurificar = false;
        podeAndar = true;

        estado = Estado.Parado;
        estaPurificado = false;
        estaAtordoado = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        empurrao = GetComponent<Empurrao>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        tremerCamera = GameObject.Find("Virtual Camera").GetComponent<TremerCamera>();
    }

    private void Update()
    {
        if (estaAtordoado && Input.GetKeyDown(KeyCode.Q) && estaNaRange)
            Purificar();
    }

    private void FixedUpdate()
    {
        if (podeAndar && JogadorVida.estaViva)
        {
            if (!estaAtordoado && !estaPurificado)
            {
                IrParaJogador();

                if (empurrao.serEmpurrado) { return; }
                if (movDir.x < 0) spriteRenderer.flipX = true;
                else if (movDir.x > 0) spriteRenderer.flipX = false;

                rb.MovePosition(rb.position + movDir * (movVel * Time.deltaTime));
            }
        }
    }

    void IrParaJogador()
    {
        Vector3 jogadorPos = JogadorController.Instance.transform.position;

        movDir = (jogadorPos - transform.position).normalized;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<JogadorVida>())
        {
            JogadorVida.Instance.LevarDano(Dano, gameObject.transform);
            tremerCamera.TremerCameraFunc();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "FlechaPlayer")
            ReceberDano(2);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
            estaNaRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
            estaNaRange = false;
    }

    public void ReceberDano(int dano)
    {
        if (Vida <= 0)
        {
            empurrao.SerEmpurrado(JogadorController.Instance.transform, empurraoForca);
            StartCoroutine(TentarPurificarRoutine());
        }
        else
        {
            Vida -= dano;
            tremerCamera.TremerCameraFunc();
        }
        //Destroy(gameObject);
    }

    IEnumerator TentarPurificarRoutine()
    {
        estaAtordoado = true;
        podePurificar = true;
        Freezar();

        yield return new WaitForSeconds(10);
        //yield return new WaitUntil(() => estaPurificado);

        if (estaPurificado)
            yield break;

        Desfrizar();
        estaAtordoado = false;
        Vida++;
    }

    void Purificar()
    {
        estaPurificado = true;
        estaAtordoado = false;
        circleCollider.enabled = false;
        gameObject.transform.tag = "Untagged";

        gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;

        Desfrizar();
        estado = Estado.Andando;
        SetVelocidade(2);
        StartCoroutine(Andando(2));
    }

    void Freezar() => rb.constraints = RigidbodyConstraints2D.FreezeAll;

    void Desfrizar()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void DiminuirVelocidade() => movVel = velInicial / 2;
    public void VoltarVelocidadeNormal() => movVel = velInicial;
}
