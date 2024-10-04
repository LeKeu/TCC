using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CucaMenino : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movDir;
    float movVel = 4.3f; 
    float distMaxJogador = 1f;
    bool estaFreezado;
    bool estaLonge;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Andar();
        if (estaLonge)
            rb.MovePosition(rb.position + movDir * (movVel * Time.deltaTime));
    }

    void Andar()
    {
        Vector3 jogadorPos = JogadorController.Instance.transform.position;

        if (Vector3.Distance(transform.position, jogadorPos) <= distMaxJogador)
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

}
