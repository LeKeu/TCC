using System.Collections;
using UnityEngine;

public class JogadorController : Singleton<JogadorController>
{
    [SerializeField] public Sprite perfil;
    public bool OlhandoEsq { get { return olhandoEsq; } }

    [SerializeField] private float velocidade = 1f;
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
    public bool estaSendoPerseguido = false;
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
        rb.MovePosition(rb.position + movimento * (velocidade * Time.fixedDeltaTime));
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
            estaDashing = true;
            velocidade *= velDash;
            trailRenderer.emitting = true;
            StartCoroutine(AcabarDash());
        }
    }

    private IEnumerator AcabarDash()
    {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        velocidade = velInicial;
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        estaDashing = false;
    }

}
