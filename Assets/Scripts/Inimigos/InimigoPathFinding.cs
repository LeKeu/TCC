using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoPathFinding : MonoBehaviour
{
    [SerializeField] private float movVel = 2f;
    [SerializeField] private float pegarDist = 5f;

    Rigidbody2D rb;
    private Vector2 movDirecao;
    private Empurrao empurrao;

    private void Awake()
    {
        empurrao = GetComponent<Empurrao>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (empurrao.serEmpurrado) { return; }
        rb.MovePosition(rb.position + movDirecao * (movVel * Time.deltaTime));
    }

    public void IrPara(Vector2 posAlvo)
    {
        Vector3 jogadorPos = JogadorController.Instance.transform.position;

        if (Vector3.Distance(transform.position, jogadorPos) < pegarDist)
            movDirecao = (jogadorPos - transform.position).normalized;
        else
            movDirecao = posAlvo;
    }
}