using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JogadorController : MonoBehaviour
{
    public static JogadorController Instance;
    public bool OlhandoEsq { get { return olhandoEsq; } set { olhandoEsq = value; } }
    [SerializeField] private float velocidade = 1f;

    private JogadorControls jogadorControls;
    private Vector2 movimento;
    private Rigidbody2D rb;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool olhandoEsq = false;

    private void Awake()
    {
        Instance = this;
        jogadorControls = new JogadorControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    private void JogadorInput()
    {
        movimento = jogadorControls.Movement.Move.ReadValue<Vector2>();

        animator.SetFloat("moveX", movimento.x);
        animator.SetFloat("moveY", movimento.y);
    }

    private void Movimentar()
    {
        rb.MovePosition(rb.position + movimento * (velocidade * Time.fixedDeltaTime));
    }

    private void AjustarJogadorEncarandoLado()  // ver segundo
    {
        Vector3 mousePosicao = Input.mousePosition;
        Vector3 jogadorScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if(mousePosicao.x < jogadorScreenPoint.x) { spriteRenderer.flipX = true; OlhandoEsq = true; }
        else { spriteRenderer.flipX = false; OlhandoEsq = false; }

    }

}
