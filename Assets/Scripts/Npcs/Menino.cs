using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Menino : MonoBehaviour
{
    [SerializeField] public float movVel = 2f;
    [SerializeField] float distMaxJogador = 1f;

    #region Parametros de Dialogo
    [Header("Dialogo")]
    [SerializeField] Sprite perfil;
    [SerializeField] private DialogoController dialogoController;
    [SerializeField] private List<DialogoTexto> dt;
    int indexAtual = 0;
    #endregion

    Rigidbody2D rb;
    Vector2 movDir;
    public bool estaLonge;

    public bool podeMover;
    public static bool acabouFalar; // usado apenas logo no início, esperando para falar com a menina
    bool estaFreezado;

    Pedrinho pedrinho;

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "01_comunidade")
            pedrinho = GameObject.Find("Pedrinho").GetComponent<Pedrinho>();
        rb = GetComponent<Rigidbody2D>();
        podeMover = SceneManager.GetActiveScene().name == "01_comunidade" ? false : true;
        estaLonge = true;
    }

    private void Andar()
    {
        IrPara();
    }

    private void Update()
    {
        if (!acabouFalar && Mouse.current.middleButton.wasPressedThisFrame && Etapas.MeninaTocandoUkulele)
            Interagir();

        if (Mouse.current.middleButton.wasPressedThisFrame && Etapas.BrigaCelebracao && !JogadorController.Instance.acabouDialogo)
            Interagir_CelebracaoCutscene();

        if (Mouse.current.middleButton.wasPressedThisFrame && Etapas.CucaSequestro && !JogadorController.Instance.acabouDialogo) // organizar isso depois
            Interagir_CelebracaoCutscene();

    }

    void FixedUpdate()
    {
        if (podeMover /*&& JogadorController.Instance.podeMover*/)
        {
            if(pedrinho?.podePegarBola == false || SceneManager.GetActiveScene().name == "02_comunidade") // se a menina for atrás da bola, ele fica esperando
                Andar();
                if (estaLonge)
                    rb.MovePosition(rb.position + movDir * (movVel * Time.deltaTime));
        }
    }

    public void Interagir()
    {
        Falar(dt[indexAtual]);
    }

    public void Interagir_CelebracaoCutscene(int index=0)
    {
        // 0 - primeiro momento da briga
        // 1 - 
        //Debug.Log($"dt count {dt.Count}");
        //Debug.Log($"index {index}");
        Falar(dt[index]);
    }

    public void Falar(DialogoTexto dialogoTexto)
    {
        dialogoTexto.perfilNPC = perfil;
        dialogoController.DisplayProximoParagrafo(dialogoTexto);
    }

    void IrPara()
    {
        Vector3 jogadorPos = JogadorController.Instance.transform.position;
        
        if(Vector3.Distance(transform.position, jogadorPos) <= distMaxJogador)
        { // perto do player
            estaLonge = false;
            if (!estaFreezado)
                FreezarMov();
        }
        else
        {
            estaLonge = true;
            movDir = (jogadorPos - transform.position).normalized;
            if (estaFreezado)
                DesfreezarMov();
        }
    }

    void FreezarMov() { rb.constraints = RigidbodyConstraints2D.FreezeAll; estaFreezado = true; }
    void DesfreezarMov() { rb.constraints = RigidbodyConstraints2D.None; rb.constraints = RigidbodyConstraints2D.FreezeRotation; estaFreezado = false; }

    public void PararDeSeguir() => podeMover = false;
    public void VoltarASeguir() => podeMover = true;

}
