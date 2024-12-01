using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espada : MonoBehaviour, IArma
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimPontoSpawn;
    //[SerializeField] private Transform armaCollider;
    //[SerializeField] private float espadaAtaqueCD = .5f;
    [SerializeField] private Armas armasInfo;

    Transform armaCollider;
    private Animator animator;
    //private JogadorController JogadorController;
    //private JogadorControls jogadorControls;
    //private ArmaAtiva armaAtiva;
    //private bool ataqueButBaixo, estaAtacando = false;

    private GameObject slahsAnim;

    SFX sfx_script;


    private void Awake()
    {
        //JogadorController = GetComponentInParent<JogadorController>();
        //armaAtiva = GetComponentInParent<ArmaAtiva>();
        animator = GetComponent<Animator>();
        //jogadorControls = new JogadorControls();
    }

    //private void OnEnable() { jogadorControls.Enable(); }

    void Start()
    {
        //jogadorControls.Combat.Attack.started += _ => Atacar();
        armaCollider = JogadorController.Instance.PegarArmaCollider();
        slashAnimPontoSpawn = GameObject.Find("splashSpawnPoint").transform;
        sfx_script = GameObject.Find("AudioSource").GetComponent<SFX>();
    }

    void Update()
    {
        MouseSeguirOffSet();
        //Atacar();
    }

    public Armas PegarArmaInfo()
    {
        return armasInfo;
    }

    public void Atacar()
    {
        StartCoroutine(IniciarAudioAtaque());
        animator.SetTrigger("Ataque");
        armaCollider.gameObject.SetActive(true);

        slahsAnim = Instantiate(slashAnimPrefab, slashAnimPontoSpawn.position, Quaternion.identity);
        slahsAnim.transform.parent = this.transform.parent;
        //StartCoroutine(AtacarCDRoutina());
    }

    IEnumerator IniciarAudioAtaque()
    {
        sfx_script.JogadorAtacando();
        yield return new WaitForSeconds(.1f);
        sfx_script.JogadorEspadaAtaques();
    }

    //IEnumerator AtacarCDRoutina()
    //{
    //    yield return new WaitForSeconds(espadaAtaqueCD);
    //    ArmaAtiva.Instance.ToggleEstaAtacando(false);
    //}

    public void FinalAnimAtacarEvent() { armaCollider.gameObject.SetActive(false); }

    public void SwingCimaFlipAnimEvent()
    {
        slahsAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (JogadorController.Instance.OlhandoEsq)
            slahsAnim.GetComponent<SpriteRenderer>().flipX = true;
    }
    public void SwingBaixoFlipAnimEvent()
    {
        slahsAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (JogadorController.Instance.OlhandoEsq)
            slahsAnim.GetComponent<SpriteRenderer>().flipX = true;
    }



    private void MouseSeguirOffSet()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 jogadorScreenPoint = Camera.main.WorldToScreenPoint(JogadorController.Instance.transform.position);

        float angulo = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < jogadorScreenPoint.x)
        { ArmaAtiva.Instance.transform.rotation = Quaternion.Euler(0, -180, angulo);
            armaCollider.transform.rotation = Quaternion.Euler(0, -180, 0); }
        else 
        {
            ArmaAtiva.Instance.transform.rotation = Quaternion.Euler(0, 0, angulo); 
            armaCollider.transform.rotation = Quaternion.Euler(0, 0, 0); }

    }

}
