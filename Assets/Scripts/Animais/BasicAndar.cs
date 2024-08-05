using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAndar : MonoBehaviour
{
    public enum Estado
    {
        Andando,
        Parado
    }

    public Vector2 movDirecao;
    public Rigidbody2D rb;
    public Estado estado;

    public IEnumerator Andando(float tempo)
    {
        while (estado == Estado.Andando)
        {
            float x = Random.Range(-1f, 1f);
            //float y = Random.Range(-1f, 1f);

            //gameObject.transform.localScale = x*100 < gameObject.transform.position.x ? new Vector2(-1, 1) : new Vector2(1, 1);

            Vector2 andandoPos = new Vector2(x, 1).normalized;
            yield return new WaitForSeconds(tempo);
            IrPara(andandoPos);
        }
    }

    void IrPara(Vector2 posAlvo)
    {
        movDirecao = posAlvo;
        if (movDirecao.x < 0) gameObject.transform.localScale = new Vector2(1, 1);
        else gameObject.transform.localScale = new Vector2(-1, 1);
    }
}
