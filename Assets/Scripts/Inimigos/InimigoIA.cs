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
        inimigoPathFinding.MoverPara(andandoPos);

        if (Vector3.Distance(transform.position, jogadorPos) < inimigoPathFinding.pegarDist && !JogadorController.Instance.estaEscondido && inimigoVida.estaCorrompido)
        {
            JogadorController.Instance.estaSendoPerseguido = true;
            estado = Estado.Atacando;
        }

        if (tempoAndando > rcdf)
        {
            andandoPos = GetAndandoPos();
        }
    }

    void Atacando()
    {
        Vector3 jogadorPos = JogadorController.Instance.transform.position;
        if (Vector3.Distance(transform.position, jogadorPos) > inimigoPathFinding.pegarDist /*&& inimigoVida.estaCorrompido*/)
        {
            JogadorController.Instance.estaSendoPerseguido = false;
            estado = Estado.Andando;
        }

        if(inimigoPathFinding.pegarDist != 0 && inimigoVida.estaCorrompido && podeAtacarAux)
        {
            podeAtacarAux = false;
            (tipoInimigo as IInimigo).Atacar();

            if (pararQndAtaca) inimigoPathFinding.PararMover(); // inimigos que param de se mover ao ver o player, ex: atirador
            else inimigoPathFinding.MoverPara((jogadorPos - transform.position).normalized); // inimigos que se movem em direção ao player

            StartCoroutine(AtaqueCooldownRoutine());
        }
    }

    private IEnumerator AtaqueCooldownRoutine()
    {
        yield return new WaitForSeconds(ataqueCooldown);
        podeAtacarAux = true;
    }

    private Vector2 GetAndandoPos()
    {
        tempoAndando = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    public void VirarPurificado()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        inimigoVida.estaCorrompido = false;
        inimigoPathFinding.movVel = 0f;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        gameObject.GetComponent<Animator>().SetBool("purificado", true);
    }
}
