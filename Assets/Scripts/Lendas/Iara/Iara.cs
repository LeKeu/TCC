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
        Direto,
        Nenhum
    }

    Estado estado;
    TipoAtaque tipoAtaque;

    Rigidbody2D rb;
    BalaSpawner balaSpawner;
    BarraVidaBosses barraVidaBosses;
    List<TipoAtaque> tipoAtaqueLista = new List<TipoAtaque>() { TipoAtaque.Perseguidor, TipoAtaque.Direto, TipoAtaque.Nenhum };

    string nome = "Iara";
    [SerializeField] int Vida = 100; // precisa ser divisível por 4! dar um número inteiro! batalha eé dividida em 4 etapas!
    [SerializeField] List<GameObject> PosLagoFunda;
    [SerializeField] GameObject PosLagoCentro;
    [SerializeField] GameObject AtqAreaIara;
    [SerializeField] float velocidade;

    [SerializeField] GameObject AreaJatoDireto;

    bool Boss1;
    bool estaNoCentro;
    bool chamandoDistante;
    bool chamandoCentro;
    
    bool estaAtqPerseguidor;
    bool estaAtqDireto;
    bool estaAtqNenhum;

    int posAnterior = 0;
    int vidaAtual;
    int auxVida;
    int indexAtq = 0;

    public Vector2 movDirecao;


    private void Start()
    {
        barraVidaBosses = GameObject.Find("Geral").GetComponent<BarraVidaBosses>();

        rb = GetComponent<Rigidbody2D>();
        balaSpawner = GetComponent<BalaSpawner>();
        estado = Estado.Distante;
        tipoAtaque = TipoAtaque.Direto;
        vidaAtual = Vida;
        auxVida = Vida / 4;
        
        Boss1 = true;
    }

    private void Update()
    {
        EstadosBoss1();
        if(Boss1 && estado == Estado.Centro) 
        { rb.MovePosition(rb.position + movDirecao * (velocidade * Time.fixedDeltaTime)); }
    }

    void EstadosBoss1()
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
                StartCoroutine(EstadoCentro());
        }
    }

    void ChecarFaseBoss1()
    {
        if (vidaAtual > Vida - auxVida) //se a vida atual for maior que 75
        {
            estado = Estado.Centro; // mudar os tipos
        }
        else if (vidaAtual > Vida - auxVida * 2 && vidaAtual <= Vida - auxVida) //se a vida atual for maior que 50 e menor ou igual a 75
        {
            estado = Estado.Distante;
        }
        else if (vidaAtual > Vida - auxVida * 3 && vidaAtual <= Vida - auxVida * 2)
        {
            estado = Estado.Centro;
        }
        else
        {
            estado = Estado.Distante;
        }
    }

    #region ATQ DISTANTE
    IEnumerator EstadoDistante()
    {
        estaNoCentro = false;
        chamandoDistante = true;

        Teletransportar();
        balaSpawner.IniciarTiros();

        yield return new WaitForSeconds(Random.Range(3, 11));

        balaSpawner.PararTiros();
        chamandoDistante = false;
    }
    #endregion

    #region ATQ CENTRO

    IEnumerator EstadoCentro()
    {
        if(estado == Estado.Centro)
        {
            chamandoCentro = true;
            if (!estaNoCentro)
                transform.position = PosLagoCentro.transform.position;
            estaNoCentro = true;

            Debug.Log(tipoAtaque.ToString());
            switch (tipoAtaque)
            {
                case TipoAtaque.Perseguidor:
                    if (!estaAtqPerseguidor)
                        StartCoroutine(AtqPerseguidor());
                    TrocarAtaque();
                    break;

                case TipoAtaque.Direto:
                    if (!estaAtqDireto)
                        StartCoroutine(AtqDireto());
                    TrocarAtaque();
                    break;

                case TipoAtaque.Nenhum:
                    if (!estaAtqNenhum)
                        StartCoroutine(AtqNenhum());
                    TrocarAtaque();
                    break;
            }
            yield return new WaitForSeconds(10);
            chamandoCentro = false;
        }
        
    }

    IEnumerator AtqPerseguidor()
    {
        estaAtqPerseguidor = true;

        if(GameObject.FindGameObjectsWithTag("AtqAreaIara").Length < 3)
        {
            yield return new WaitForSeconds(.5f);
            Instantiate(AtqAreaIara, JogadorController.Instance.transform.position, Quaternion.identity);
        }
        estaAtqPerseguidor = false;
    }

    IEnumerator AtqNenhum()
    {
        estaAtqNenhum = true;
        yield return new WaitForSeconds(3);
        estaAtqNenhum = false;
    }

    IEnumerator AtqDireto()
    {
        estaAtqDireto = true;
        GameObject areaJato = Instantiate(AreaJatoDireto, transform.position, Quaternion.identity);

        yield return new WaitUntil(() => areaJato.GetComponent<AtqDiretoJato>().CompletouRot());
        areaJato.GetComponent<AtqDiretoJato>().DestruirArea();
           
    }

    

    #endregion

    void TrocarAtaque()
    {
        if(indexAtq == tipoAtaqueLista.Count)
            indexAtq = 0;
        tipoAtaque = tipoAtaqueLista[indexAtq];
        Debug.Log($"novo atq:{tipoAtaque.ToString()}");
        indexAtq++;
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
