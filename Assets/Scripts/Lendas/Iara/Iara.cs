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

    public enum TipoAtaque
    {
        Perseguidor,
        Direto
    }

    Estado estado;
    TipoAtaque tipoAtaque;

    Rigidbody2D rb;
    BalaSpawner balaSpawner;
    BarraVidaBosses barraVidaBosses;
    List<TipoAtaque> tipoAtaqueLista = new List<TipoAtaque>() { TipoAtaque.Perseguidor, TipoAtaque.Direto };

    string nome = "Iara";
    [SerializeField] int Vida = 100; // precisa ser divisível por 4! dar um número inteiro! batalha eé dividida em 4 etapas!
    [SerializeField] List<GameObject> PosLagoFunda;
    [SerializeField] GameObject PosLagoCentro;
    [SerializeField] GameObject AtqAreaIara;
    [SerializeField] float velocidade;

    bool Boss1;
    bool estaNoCentro;
    bool andandoCentro;
    bool chamandoDistante;
    bool chamandoCentro;
    bool estaAtqPerseguidor;
    

    int posAnterior = 0;
    int vidaAtual;
    int auxVida;

    float timer = 0f;
    public Vector2 movDirecao;


    private void Start()
    {
        barraVidaBosses = GameObject.Find("Geral").GetComponent<BarraVidaBosses>();

        rb = GetComponent<Rigidbody2D>();
        balaSpawner = GetComponent<BalaSpawner>();
        estado = Estado.Distante;
        tipoAtaque = TipoAtaque.Perseguidor;
        Boss1 = true;
        vidaAtual = Vida;
        auxVida = Vida / 4;
        EstadosBoss1();
    }

    private void Update()
    {
        if(Boss1 && estado == Estado.Centro) 
        { rb.MovePosition(rb.position + movDirecao * (velocidade * Time.fixedDeltaTime)); }
    }

    void ChecarFaseBoss1()
    {
        if (vidaAtual > Vida - auxVida) //se a vida atual for maior que 75
        {
            estado = Estado.Distante; // mudar os tipos
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
        while (Boss1)
        {
            ChecarFaseBoss1();

            if (!barraVidaBosses.ContainerEstaAtivo()) // criar a barra de vida do saci
                barraVidaBosses.CriarContainer(Vida, nome);
            //Debug.Log(estado.ToString());
            if (Vida > 0)
            {
                if (estado == Estado.Distante && !chamandoDistante)
                    StartCoroutine(EstadoDistante());

                if (estado == Estado.Centro && !chamandoCentro)
                    EstadoCentro();
            }
        }
    }

    IEnumerator EstadoDistante()
    {
        estaNoCentro = false;

        Freezar();
        chamandoDistante = true;
        Teletransportar();
        balaSpawner.IniciarTiros();
        yield return new WaitForSeconds(Random.Range(3, 11));
        balaSpawner.PararTiros();
        chamandoDistante = false;
    }

    void EstadoCentro()
    {
        if(!estaNoCentro)
            gameObject.transform.position = PosLagoCentro.transform.position;
        switch (tipoAtaque)
        {
            case TipoAtaque.Perseguidor:
                if(!estaAtqPerseguidor)
                    StartCoroutine(InstanciarAtqArea());
                StartCoroutine(TrocarAtaque());
                estaAtqPerseguidor = false;
                break;
            case TipoAtaque.Direto:
                Debug.Log("direto"); 
                StartCoroutine(TrocarAtaque());
                break;
        }
        
        estaNoCentro = true;
    }

    IEnumerator TrocarAtaque()
    {
        tipoAtaque = tipoAtaqueLista[Random.Range(0, tipoAtaqueLista.Count)];
        yield return new WaitForSeconds(3);
    }

    IEnumerator InstanciarAtqArea()
    {
        chamandoCentro = true;
        estaAtqPerseguidor = true;

        Instantiate(AtqAreaIara, JogadorController.Instance.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1);
        
        chamandoCentro = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "AguaIaraCollider")
            movDirecao *= -1;
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
        if (vidaAtual >= 0)
        {
            vidaAtual -= dano;
            barraVidaBosses.ReceberDano(dano);
        }
        else Boss1 = false;
    }

    void Freezar() => rb.constraints = RigidbodyConstraints2D.FreezeAll;

    void Desfrizar()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
