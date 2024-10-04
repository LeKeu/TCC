using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CucaMenino : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movDir;
    [SerializeField] float movVel = 1;//= 4.3f;
    [SerializeField] float distMaxJogador = 1f;
    bool estaFreezado;
    bool estaLonge;

    [SerializeField] private float sumirTempo = .4f;
    [SerializeField] private float qntdTransparencia = .8f;

    Transparencia transparencia;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transparencia = GetComponent<Transparencia>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Andar();
        if (estaLonge)
            rb.MovePosition(rb.position + movDir * (movVel * Time.deltaTime));
    }

    void Andar()
    {
        Vector3 jogadorPos = JogadorController.Instance.transform.position;

        if (Vector3.Distance(transform.position, jogadorPos) <= distMaxJogador)
        { // perto do player
            estaLonge = false;
            if (!estaFreezado)
                FreezarMov();
        }
        else
        {
            estaLonge = true;
            movDir = (jogadorPos - transform.position).normalized;
            if (estaFreezado)
                DesfreezarMov();
        }
    }

    void FreezarMov() { rb.constraints = RigidbodyConstraints2D.FreezeAll; estaFreezado = true; }
    void DesfreezarMov() { rb.constraints = RigidbodyConstraints2D.None; rb.constraints = RigidbodyConstraints2D.FreezeRotation; estaFreezado = false; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<JogadorController>() || collision.gameObject.GetComponent<Projetil>())
        {
            StartCoroutine(transparencia.SumirSpriteIndividual(spriteRenderer, sumirTempo, spriteRenderer.color.a, qntdTransparencia));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<JogadorController>() || collision.gameObject.GetComponent<Projetil>())
        {
            StartCoroutine(transparencia.AparecerSpriteIndividual(spriteRenderer, sumirTempo, spriteRenderer.color.a, 1));
        }
    }
}
