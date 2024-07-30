using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garca : BasicAndar
{

    [SerializeField] float movVel = 1f;
    [SerializeField] float tempoParado = 2f;

    void Awake()
    {
        estado = Estado.Andando;
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        StartCoroutine(Andando(tempoParado));
    }

    void FixedUpdate()
    {
        Vector2 teste = movDirecao * (movVel * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + teste);
    }

    
}
