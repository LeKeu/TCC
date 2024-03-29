using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoPathFinding : MonoBehaviour
{
    [SerializeField] private float movVel = 2f;

    Rigidbody2D rb;
    private Vector2 movDirecao;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movDirecao * (movVel * Time.deltaTime));
    }

    public void IrPara(Vector2 posAlvo)
    {
        movDirecao = posAlvo;
    }
}
