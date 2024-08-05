using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoIA : MonoBehaviour
{
    [SerializeField] private float rcdf = 2f; //roamchangedirfloat
    [SerializeField] private MonoBehaviour tipoInimigo;
    [SerializeField] bool pararQndAtaca = false;
    [SerializeField] float ataqueCooldown = 2f;
    public enum Estado
    {
        Andando,
        Atacando
    }

    public Estado estado;
    private InimigoPathFinding inimigoPathFinding;
    InimigoVida inimigoVida;
    Vector2 andandoPos;
    float tempoAndando = 0f;

    bool podeAtacarAux = true;

    private void Awake()
    {
        inimigoPathFinding = GetComponent<InimigoPathFinding>();
        inimigoVida = GetComponent<InimigoVida>();
        estado = Estado.Andando;
    }

    void Start()
    {
        andandoPos = GetAndandoPos();
    }

    private void Update()
    {
        MovControl();
    }

    void MovControl()
    {
        switch (estado)
        {
            default:
                case Estado.Andando:
                    Andando(); break;
                case Estado.Atacando:
                    Atacando(); break;
        }
    }

    public void Andando()
    {
        tempoAndando += Time.deltaTime;
        Vector3 jogadorPos = JogadorController.Instance.transform.position;
        //Debug.Log("andando");
        inimigoPathFinding.MoverPara(andandoPos);

        if (Vector3.Distance(transform.position, jogadorPos) < inimigoPathFinding.pegarDist && !JogadorController.Instance.estaEscondido && inimigoVida.estaCorrompido)
        {
            //inimigoPathFinding.movDirecao = (jogadorPos - transform.position).normalized;
            JogadorController.Instance.estaSendoPerseguido = true;
            estado = InimigoIA.Estado.Atacando;
            Debug.Log("vou atyacar");
        }
        //else
        //{
        //    estado = InimigoIA.Estado.Andando;
        //    inimigoPathFinding.movDirecao = posAlvo;
        //    JogadorController.Instance.estaSendoPerseguido = false;
        //}

        if (tempoAndando > rcdf)
        {
            //Debug.Log("jhsgjdhag");
            andandoPos = GetAndandoPos();
        }
    }

    void Atacando()
    {
        Debug.Log("ATACANDO");
        Vector3 jogadorPos = JogadorController.Instance.transform.position;
        if (Vector3.Distance(transform.position, jogadorPos) > inimigoPathFinding.pegarDist && inimigoVida.estaCorrompido)
        {
            //Debug.Log("mudando andando");
            estado = InimigoIA.Estado.Andando;
            //inimigoPathFinding.movDirecao = posAlvo;
            JogadorController.Instance.estaSendoPerseguido = false;
        }

        if(inimigoPathFinding.pegarDist != 0 && inimigoVida.estaCorrompido && podeAtacarAux)
        {
            podeAtacarAux = false;
            (tipoInimigo as IInimigo).Atacar();

            if (pararQndAtaca) inimigoPathFinding.PararMover();
            else inimigoPathFinding.MoverPara(andandoPos);

            StartCoroutine(AtaqueCooldownRoutine());
        }
    }

    private IEnumerator AtaqueCooldownRoutine()
    {
        yield return new WaitForSeconds(ataqueCooldown);
        podeAtacarAux = true;
    }

    //private IEnumerator AndandoOutLine()
    //{
    //    while(estado == Estado.Andando)
    //    {
    //        andandoPos = GetAndandoPos();
    //        Andando();//
    //        yield return new WaitForSeconds(rcdf);
    //    }
    //}

    private Vector2 GetAndandoPos()
    {
        tempoAndando = 0f;
        Debug.Log("andando MUDEI");
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    public void VirarPurificado()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        gameObject.GetComponent<InimigoVida>().estaCorrompido = false;
        gameObject.GetComponent<InimigoPathFinding>().movVel = .2f;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }
}
