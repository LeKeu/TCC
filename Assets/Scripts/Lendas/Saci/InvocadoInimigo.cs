using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvocadoInimigo : MonoBehaviour
{
    [SerializeField] public float movVel = 2f;
    [SerializeField] public float pegarDist = 2f;

    Rigidbody2D rb;
    Vector2 movDir;
    private Empurrao empurrao;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        empurrao = GetComponent<Empurrao>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        IrPara();
        if (empurrao.serEmpurrado) { return; }
        if (movDir.x < 0) spriteRenderer.flipX = true;
        else if (movDir.x > 0) spriteRenderer.flipX = false;

        rb.MovePosition(rb.position + movDir * (movVel * Time.deltaTime));

    }

    void IrPara()
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
}
