using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoPathFinding : MonoBehaviour
{
    [SerializeField] public float movVel = 2f;
    [SerializeField] public float pegarDist = 2f;

    Rigidbody2D rb;
    InimigoVida InimigoVida;
    InimigoIA InimigoIA;
    public Vector2 movDirecao;
    private Empurrao empurrao;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InimigoVida = GetComponent<InimigoVida>();
        InimigoIA = GetComponent<InimigoIA>();
        empurrao = GetComponent<Empurrao>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!InimigoVida.estaAtordoado) // se não tiver atordoado, pode ser mexer
        {
            if (empurrao.serEmpurrado) { return; }
            rb.MovePosition(rb.position + movDirecao * (movVel * Time.deltaTime));
            
            if(movDirecao.x < 0) spriteRenderer.flipX = true;
            else if(movDirecao.x > 0) spriteRenderer.flipX = false;
        }
    }

    public void MoverPara(Vector2 alvoPos)
    {
        movDirecao = alvoPos;
    }

    public void PararMover()
    {
        movDirecao = Vector3.zero;
    }
}