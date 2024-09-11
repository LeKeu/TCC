using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
    BalaSpawner balaSpawner;
    BarraVidaBosses barraVidaBosses;

    string nome = "Iara";
    [SerializeField] int Vida = 100; // precisa ser divisível por 4! dar um número inteiro! batalha eé dividida em 4 etapas!
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
        barraVidaBosses = GameObject.Find("Geral").GetComponent<BarraVidaBosses>();

        rb = GetComponent<Rigidbody2D>();
        balaSpawner = GetComponent<BalaSpawner>();
        estado = Estado.Distante;
        Boss1 = true;
        vidaAtual = Vida;
        auxVida = Vida / 4;

    }

    private void Update()
    {
        EstadosBoss1();
    }

    void ChecarFaseBoss1()
    {
        if (vidaAtual > Vida - auxVida) //se a vida atual for maior que 75
        {
            estado = Estado.Distante;
        }else if(vidaAtual > Vida - auxVida*2 && vidaAtual <= Vida - auxVida) //se a vida atual for maior que 50 e menor ou igual a 75
        {
            estado = Estado.Centro;
        }
        else if(vidaAtual > Vida - auxVida*3 && vidaAtual <= Vida - auxVida * 2)
        {
            estado = Estado.Distante;
        }
        else
        {
            estado = Estado.Centro;
        }
    }

    void EstadosBoss1()
    {
        ChecarFaseBoss1();

        if (!barraVidaBosses.ContainerEstaAtivo()) // criar a barra de vida do saci
            barraVidaBosses.CriarContainer(Vida, nome);

        if(Vida > 0)
        {
            if (estado == Estado.Distante && !chamandoDistante)
                StartCoroutine(EstadoDistante());
        
            if(estado == Estado.Centro && !chamandoCentro)
                StartCoroutine(EstadoCentro());
        }
        
    }

    IEnumerator EstadoDistante()
    {
        chamandoDistante = true;
        Teletransportar();
        balaSpawner.IniciarTiros();
        yield return new WaitForSeconds(Random.Range(3, 11));
        balaSpawner.PararTiros();
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
        if(vidaAtual >= 0)
        {
            vidaAtual -= dano;
            barraVidaBosses.ReceberDano(dano);
        }
    }
}
