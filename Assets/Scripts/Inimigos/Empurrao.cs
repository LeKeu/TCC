using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empurrao : MonoBehaviour
{
    public bool serEmpurrado { get; private set; }
    [SerializeField] private float tempoEmpurrao = .2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SerEmpurrado(Transform origemDano, float empurrao)
    {
        serEmpurrado = true;
        Vector2 differenca = (transform.position - origemDano.position).normalized * empurrao * rb.mass;
        rb.AddForce(differenca, ForceMode2D.Impulse);
        StartCoroutine(EmpurrarRotina());
    }

    private IEnumerator EmpurrarRotina()
    {
        yield return new WaitForSeconds(tempoEmpurrao);
        rb.velocity = Vector2.zero;
        serEmpurrado = false;
    }
}
