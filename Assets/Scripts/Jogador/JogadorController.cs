using System.Collections;
using UnityEngine;

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

    public bool estaEscondido = false;
    public bool estaNaAgua = false;
    public bool estaSendoPerseguido = false;
    public bool estaAndando;
    public bool podeMover;

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
        jogadorControls.Combat.Dash.performed += _ => Dash();

        velInicial = velocidade;
    }

    private void OnEnable()
    {
        jogadorControls.Enable();
    }

    private void Update()
    {
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

        if (estaEscondido || estaNaAgua) DiminuirVelocidade();// se tiver stealth, diminui a velocidade
        else if(!estaDashing) VoltarVelocidadeNormal();

        if (movimento == Vector2.zero) estaAndando = false;
        else estaAndando = true;

        rb.MovePosition(rb.position + movimento * (velocidade * Time.fixedDeltaTime));
    }

    public void DiminuirVelocidade()
    {
        //Debug.Log("diminuindo vel");
        velocidade = velInicial / 2;
    }

    public void VoltarVelocidadeNormal()
    {
        //Debug.Log("voltando vel");
        velocidade = velInicial;
    }

    private void AjustarJogadorEncarandoLado()  // ver segundo
    {
        Vector3 mousePosicao = Input.mousePosition;
        Vector3 jogadorScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if(mousePosicao.x < jogadorScreenPoint.x) { spriteRenderer.flipX = true; olhandoEsq = true; }
        else { spriteRenderer.flipX = false; olhandoEsq = false; }
    }

    private void Dash()
    {
        if (!estaDashing && podeMover)
        {
            estaDashing = true; //JogadorVida.Instance.podeLevarDano = false;
            velocidade *= velDash;
            Debug.Log("vel antes acabar dash "+ velocidade);
            trailRenderer.emitting = true;
            StartCoroutine(AcabarDash());
        }
    }

    private IEnumerator AcabarDash()
    {
        gameObject.GetComponent<JogadorVida>().podeLevarDano = false;
        float dashTime = .2f;
        float dashCD = .25f; // cooldown
        yield return new WaitForSeconds(dashTime);
        Debug.Log("vel depois acabar dash " + velocidade);

        gameObject.GetComponent<JogadorVida>().podeLevarDano = true;
        velocidade = velInicial;
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        estaDashing = false; //JogadorVida.Instance.podeLevarDano = true;
    }

}
