using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JogadorController : Singleton<JogadorController>
{
    [SerializeField] public Sprite perfil;
    public bool OlhandoEsq { get { return olhandoEsq; } }

    [SerializeField] public float velocidade = 1f;
    [SerializeField] private float velDash = 3f;
    [SerializeField] private TrailRenderer trailRenderer;

    [SerializeField] private Transform armaCollider;

    private JogadorControls jogadorControls;
    private Vector2 movimento;
    private Rigidbody2D rb;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Empurrao empurrao;
    private float velInicial;

    private bool olhandoEsq = false;
    private bool estaDashing = false;
    public bool acabouDialogo;

    public bool estaEscondido = false;
    public bool estaNaAgua = false;
    public bool estaNaPonte = false;
    public bool estaSendoPerseguido = false;
    public bool estaAndando;
    public bool podeMover;
    public bool podeAtacar;

    protected override void Awake()
    {
        base.Awake();

        podeMover = true;
        jogadorControls = new JogadorControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        empurrao = GetComponent<Empurrao>();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "01_comunidade")
            podeAtacar = true;

        if (SceneManager.GetActiveScene().name == "01_comunidade")
            acabouDialogo = false; // se no inicio de uma cena for uma cutscene, pode come�ar assim que ela n�o vai se mover
        else acabouDialogo = true;

        jogadorControls.Combat.Dash.performed += _ => Dash();

        velInicial = velocidade;
    }

    private void OnEnable()
    {
        jogadorControls.Enable();
    }

    private void Update()
    {//jogador controller
        if (!acabouDialogo)
            podeMover = false;
        else podeMover = true;

        JogadorInput();
    }
    private void FixedUpdate()
    {
        AjustarJogadorEncarandoLado();
        Movimentar();
    }

    public Transform PegarArmaCollider()
    {
        return armaCollider;
    }


    private void JogadorInput()
    {
        if (podeMover)
        {
            movimento = jogadorControls.Movement.Move.ReadValue<Vector2>();

            animator.SetFloat("moveX", movimento.x);
            animator.SetFloat("moveY", movimento.y);
        }
    }

    private void Movimentar()
    {
        if (empurrao.serEmpurrado) { return; }

        if ((estaEscondido || estaNaAgua) && !estaNaPonte) DiminuirVelocidade();// se tiver stealth, diminui a velocidade
        else if(!estaDashing) VoltarVelocidadeNormal();

        if (movimento == Vector2.zero) estaAndando = false;
        else estaAndando = true;

        rb.MovePosition(rb.position + movimento * (velocidade * Time.fixedDeltaTime));
    }

    public void DiminuirVelocidade() => velocidade = velInicial / 2;

    public void VoltarVelocidadeNormal() => velocidade = velInicial;

    private void AjustarJogadorEncarandoLado()  // ver segundo
    {
        Vector3 mousePosicao = Input.mousePosition;
        Vector3 jogadorScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if(mousePosicao.x < jogadorScreenPoint.x) { spriteRenderer.flipX = true; olhandoEsq = true; }
        else { spriteRenderer.flipX = false; olhandoEsq = false; }
    }

    private void Dash()
    {
        if (!estaNaAgua)
        {
            if (!estaDashing && podeMover) // n�o pode dar dash na �gua
            {
                estaDashing = true; //JogadorVida.Instance.podeLevarDano = false;
                velocidade *= velDash;
                trailRenderer.emitting = true;
                StartCoroutine(AcabarDash());
            }
        }
    }

    private IEnumerator AcabarDash()
    {
        gameObject.GetComponent<JogadorVida>().podeLevarDano = false;
        float dashTime = .2f;
        float dashCD = .25f; // cooldown
        yield return new WaitForSeconds(dashTime);

        gameObject.GetComponent<JogadorVida>().podeLevarDano = true;
        velocidade = velInicial;
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        estaDashing = false; //JogadorVida.Instance.podeLevarDano = true;
    }

}
