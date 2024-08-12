using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Invocado : MonoBehaviour
{
    [SerializeField] public float movVel = 2f;
    [SerializeField] public float pegarDist = 2f;

    Rigidbody2D rb;
    public Vector2 movDirecao;
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
        MoverPara();
        if (empurrao.serEmpurrado) { return; }
        

        if (movDirecao.x < 0) spriteRenderer.flipX = true;
        else if (movDirecao.x > 0) spriteRenderer.flipX = false;
        
    }

    public void MoverPara()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(origin, pegarDist);

        //foreach (Collider2D c in colliders)
        //{
        //    if (Vector2.Distance(transform.position, c.transform.position) < distAux && c.GetComponent<InimigoVida>())
        //    {
        //        distAux = Vector2.Distance(transform.position, c.transform.position);
        //        movDirecao = c.transform.position;
        //        Debug.Log(c.transform.name);
        //    }
        //}
        if(colliders.Length <= 0) { PararMover(); }
        foreach (Collider2D c in colliders)
        {
            if (c.GetComponent<InimigoVida>() && c.GetComponent<InimigoVida>().estaCorrompido)
            {
                movDirecao = (c.transform.position - gameObject.transform.position).normalized;
                break;
            }
        }
        rb.MovePosition(rb.position + movDirecao * (movVel * Time.deltaTime));
    }

    public void PararMover()
    {
        movDirecao = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<InimigoVida>())
        {
            collision.GetComponent<InimigoVida>().ReceberDano(2);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pegarDist);
    }
}
