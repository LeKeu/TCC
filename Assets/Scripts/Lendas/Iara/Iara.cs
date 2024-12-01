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

    public enum TipoAtaqueCentro
    {
        Perseguidor,
        Direto,
        Nenhum
    }

    Estado estado;
    TipoAtaqueCentro tipoAtaqueCentro;

    Rigidbody2D rb;
    BalaSpawner balaSpawner;
    BarraVidaBosses barraVidaBosses;
    AjusteTamanhoCamera ajusteTamanhoCamera;

    List<TipoAtaqueCentro> tipoAtaqueLista = new List<TipoAtaqueCentro>() { TipoAtaqueCentro.Perseguidor, TipoAtaqueCentro.Direto, TipoAtaqueCentro.Nenhum };

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

    bool estaAtacandoCentro;
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

        ajusteTamanhoCamera = GameObject.Find("Virtual Camera").GetComponent<AjusteTamanhoCamera>();
        barraVidaBosses = GameObject.Find("Geral").GetComponent<BarraVidaBosses>();

        rb = GetComponent<Rigidbody2D>();
        balaSpawner = GetComponent<BalaSpawner>();
        estado = Estado.Distante;
        tipoAtaqueCentro = TipoAtaqueCentro.Direto;
        vidaAtual = Vida;
        auxVida = Vida / 4;
        
        Boss1 = true;
    }

    private void Update()
    {
        if(Boss1)
            EstadosBoss1();
        if(Boss1 && estado == Estado.Centro) 
        { rb.MovePosition(rb.position + movDirecao * (velocidade * Time.fixedDeltaTime)); }
        if (!Boss1) TamanhoCamera(3);
    }

    void EstadosBoss1()
    {
        TamanhoCamera(7);
        ChecarFaseBoss1();

        if (!barraVidaBosses.ContainerEstaAtivo()) // criar a barra de vida do saci
            barraVidaBosses.CriarContainer(Vida, nome);

        if (Vida > 0)
        {
            //Debug.Log(estado.ToString());
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
        GameObject.FindWithTag("AtqJatoCentro")?.GetComponent<AtqDiretoJato>().DestruirArea();
        ReiniciarAtaqueCentro();
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
            gameObject.GetComponent<BalaSpawner>().DestruirBalas();

            chamandoCentro = true;
            if (!estaNoCentro)
                transform.position = PosLagoCentro.transform.position;
            estaNoCentro = true;

            switch (tipoAtaqueCentro)
            {
                case TipoAtaqueCentro.Perseguidor:
                    if (!estaAtqPerseguidor)
                        StartCoroutine(AtqPerseguidor());
                    TrocarAtaque();
                    break;

                case TipoAtaqueCentro.Direto:
                    if (!estaAtqDireto)
                        StartCoroutine(AtqDireto());
                    TrocarAtaque();
                    break;

                case TipoAtaqueCentro.Nenhum:
                    if (!estaAtqNenhum)
                        StartCoroutine(AtqNenhum());
                    TrocarAtaque();
                    break;
            }
            yield return new WaitUntil(() => !estaAtacandoCentro);
            chamandoCentro = false;
        }
    }

    IEnumerator AtqPerseguidor()
    {
        estaAtacandoCentro = true;
        estaAtqPerseguidor = true;

        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(.5f);
            Instantiate(AtqAreaIara, JogadorController.Instance.transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(2);
        estaAtqPerseguidor = false;
        estaAtacandoCentro = false;
    }

    IEnumerator AtqNenhum()
    {
        estaAtacandoCentro = true;
        estaAtqNenhum = true;
        yield return new WaitForSeconds(3);
        estaAtqNenhum = false;
        estaAtacandoCentro = false;
    }

    IEnumerator AtqDireto()
    {
        estaAtacandoCentro = true;
        estaAtqDireto = true;
        GameObject areaJato = Instantiate(AreaJatoDireto, transform.position, Quaternion.identity);

        yield return new WaitUntil(() => areaJato.GetComponent<AtqDiretoJato>().CompletouRot());

        areaJato.GetComponent<AtqDiretoJato>().DestruirArea();
        estaAtqDireto = false;
        estaAtacandoCentro = false;
    }

    #endregion

    void TrocarAtaque()
    {
        if(indexAtq == tipoAtaqueLista.Count)
            indexAtq = 0;
        tipoAtaqueCentro = tipoAtaqueLista[indexAtq];
        //Debug.Log($"novo atq:{tipoAtaque.ToString()}");
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

    void TamanhoCamera(float tam)
    {
        if (!ajusteTamanhoCamera.ChecarTamanhoCamera(tam))
            ajusteTamanhoCamera.AjustarTamanhoCamera(tam);
    }
    void ReiniciarAtaqueCentro()
    {
        estaAtqDireto = false;
        estaAtqNenhum = false;
        estaAtqPerseguidor = false;
        estaAtacandoCentro = false;
        estaNoCentro = false;
    }
}
