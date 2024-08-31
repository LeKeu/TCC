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

    bool estaFreezado;
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
        if (podeMover && JogadorController.Instance.podeMover)
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

    public void PararDeSeguir() => podeMover = false;
    public void VoltarASeguir() => podeMover = true;
}
