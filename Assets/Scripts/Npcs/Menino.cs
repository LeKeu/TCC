using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Menino : MonoBehaviour
{
    [SerializeField] float movVel = 2f;
    [SerializeField] float distMaxJogador = 1f;

    Rigidbody2D rb;
    Vector2 movDir;
    bool estaLonge;

    public bool podeMover;

    Pedrinho pedrinho;
    void Start()
    {
        pedrinho = GameObject.Find("Pedrinho").GetComponent<Pedrinho>();
        rb = GetComponent<Rigidbody2D>();
        podeMover = true;
    }

    private void Andar()
    {
        IrPara();
    }

    void FixedUpdate()
    {
        if (podeMover)
        {
            if(pedrinho.podePegarBola == false) // se a menina for atrás da bola, ele fica esperando
                Andar();
                if (estaLonge)
                    rb.MovePosition(rb.position + movDir * (movVel * Time.deltaTime));
        }
    }

    void IrPara()
    {
        Vector3 jogadorPos = JogadorController.Instance.transform.position;
        
        if(Vector3.Distance(transform.position, jogadorPos) <= distMaxJogador)
            estaLonge = false;
        else
        {
            estaLonge = true;
            movDir = (jogadorPos - transform.position).normalized;
        }
    }

    public void PararDeSeguir() => podeMover = false;
    public void VoltarASeguir() => podeMover = true;
}
