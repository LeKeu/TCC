using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iara : MonoBehaviour
{
    float movVel = 2f;
    float distMaxJogador = 1f;

    Rigidbody2D rb;
    Vector2 movDir;
    bool estaLonge;

    bool estaFreezado;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Andar()
    {
        IrPara();
    }

    void FixedUpdate()
    {        
        Andar();
        
        rb.MovePosition(rb.position + movDir * (movVel * Time.deltaTime));
        
    }

    void IrPara()
    {
        Vector3 jogadorPos = JogadorController.Instance.transform.position;

        if (Vector3.Distance(transform.position, jogadorPos) <= distMaxJogador)
        { // perto do player
            if (!estaFreezado)
                FreezarMov();
        }
        else
        {
            movDir = (jogadorPos - transform.position).normalized;
            if (estaFreezado)
                DesfreezarMov();
        }
    }

    void FreezarMov() { rb.constraints = RigidbodyConstraints2D.FreezeAll; estaFreezado = true; }
    void DesfreezarMov() { rb.constraints = RigidbodyConstraints2D.None; rb.constraints = RigidbodyConstraints2D.FreezeRotation; estaFreezado = false; }

}
