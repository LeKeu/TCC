using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iara : MonoBehaviour
{
    public enum Estado
    {
        Distante,
        Centro
    }

    Estado estado;

    Rigidbody2D rb;

    BarraVidaBosses barraVidaBosses;

    [SerializeField] int Vida = 100; // precisa ser divisível por 4! dar um número inteiro!
    [SerializeField] List<GameObject> PosLagoFunda;
    [SerializeField] GameObject PosLagoCentro;

    bool Boss1;
    bool chamandoCentro;
    bool chamandoDistante;

    int posAnterior = 0;
    int vidaAtual;
    int auxVida;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        estado = Estado.Distante;
        Boss1 = true;
        vidaAtual = Vida;
        auxVida = Vida / 4;

    }

    private void Update()
    {
        EstadosBoss1();
        ChecarFaseBoss1();
    }

    void ChecarFaseBoss1()
    {
        Debug.Log($"vida atual --> {vidaAtual}");
        Debug.Log($"PRIMEIRO --> {Vida - auxVida}");
        Debug.Log($"SEGUNDO --> {Vida - auxVida*2}");
        Debug.Log($"TERCEIRO --> {Vida - auxVida*3}");

        if (vidaAtual > Vida - auxVida) //se minha vida atual for maior que 75
        {
            estado = Estado.Distante;
            Debug.Log("1");
        }else if(vidaAtual > Vida - auxVida*2 && vidaAtual <= Vida - auxVida) //se minha vida atual for maior que 50 e menor ou igual a 75
        {
            estado = Estado.Centro;
            Debug.Log("2");
        }
        else if(vidaAtual > Vida - auxVida*3 && vidaAtual <= Vida - auxVida * 2)
        {
            estado = Estado.Distante;
            Debug.Log("3");
        }
        else
        {
            estado = Estado.Centro;
            Debug.Log("4");
        }
    }

    void EstadosBoss1()
    {
        if(estado == Estado.Distante)
        {
            if(!chamandoDistante)
                StartCoroutine(EstadoDistante());
        }
        if(estado == Estado.Centro)
        {
            if (!chamandoCentro)
                StartCoroutine(EstadoCentro());
        }
    }

    IEnumerator EstadoDistante()
    {
        chamandoDistante = true;
        Teletransportar();
        yield return new WaitForSeconds(Random.Range(3, 11));
        chamandoDistante = false;
    }

    IEnumerator EstadoCentro()
    {
        chamandoCentro = true;
        gameObject.transform.position = PosLagoCentro.transform.position;
        yield return new WaitForSeconds(3);
        chamandoCentro = false;
    }

    void Teletransportar()
    {
        int pos = Random.Range(0, 4);

        if (pos == posAnterior)
            pos += pos + 1 > 3 ? -1 : 1;

        gameObject.transform.position = PosLagoFunda[pos].transform.position;
        posAnterior = pos;
    }

    public void ReceberDano(int dano)
    {
        vidaAtual -= dano;
    }
}
