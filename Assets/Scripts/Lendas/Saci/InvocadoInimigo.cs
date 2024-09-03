using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InvocadoInimigo : MonoBehaviour
{
    [SerializeField] public float movVel = 2f;
    [SerializeField] public float pegarDist = 2f;
    [SerializeField] public int Vida = 4;

    Rigidbody2D rb;
    Vector2 movDir;
    private Empurrao empurrao;
    SpriteRenderer spriteRenderer;
    CircleCollider2D circleCollider;

    bool estaPurificado;
    bool estaAtordoado;
    bool estaNaRange;

    private void Awake()
    {
        estaPurificado = false;
        estaAtordoado = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        empurrao = GetComponent<Empurrao>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void FixedUpdate()
    {
        if (!estaAtordoado && !estaPurificado)
        {
            IrParaJogador();

            if (empurrao.serEmpurrado) { return; }
            if (movDir.x < 0) spriteRenderer.flipX = true;
            else if (movDir.x > 0) spriteRenderer.flipX = false;

            rb.MovePosition(rb.position + movDir * (movVel * Time.deltaTime));
        }

        if (estaAtordoado && Input.GetKeyDown(KeyCode.Q) && estaNaRange)
            Purificar();
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
            collision.gameObject.GetComponent<JogadorVida>().LevarDano(1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
            estaNaRange = true;
        //Debug.Log("esta na range = "+estaNaRange);
        //Debug.Log("esta atorodado = "+estaAtordoado);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
            estaNaRange = false;
    }

    public void ReceberDano(int dano)
    {
        if (Vida <= 0)
            StartCoroutine(TentarPurificarRoutine());
        else
            Vida -= dano;
        //Destroy(gameObject);
    }

    IEnumerator TentarPurificarRoutine()
    {
        estaAtordoado = true;
        Freezar();

        yield return new WaitForSeconds(10);

        if (estaPurificado)
        {
            Debug.Log("hihihi");
            //Purificar();
            yield break;
        }
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

        Debug.Log("PURIFICANDOW");
        gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    void Freezar() => rb.constraints = RigidbodyConstraints2D.FreezeAll;

    void Desfrizar()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
