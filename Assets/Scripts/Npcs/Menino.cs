using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menino : MonoBehaviour
{
    [SerializeField] float movVel = 2f;
    [SerializeField] float distMaxJogador = 1f;

    Rigidbody2D rb;
    Vector2 movDir;
    bool estaLonge;

    public bool podeMover;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        podeMover = true;
    }

    private void Andar()
    {
        IrPara();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (podeMover)
        {
            Andar();
            if (estaLonge)
                rb.MovePosition(rb.position + movDir * (movVel * Time.deltaTime));
        }
    }

    void IrPara()
    {
        Vector3 jogadorPos = JogadorController.Instance.transform.position;
        
        //Debug.Log(Vector3.Distance(transform.position, jogadorPos));
        if(Vector3.Distance(transform.position, jogadorPos) <= distMaxJogador)
        {
            estaLonge = false;
        }
        else
        {
            estaLonge = true;
            movDir = (jogadorPos - transform.position).normalized;
        }
    }
}
