using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espada : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimPontoSpawn;
    [SerializeField] private Transform armaCollider;

    private JogadorController JogadorController;
    private Animator animator;
    private JogadorControls jogadorControls;
    private ArmaAtiva armaAtiva;

    private GameObject slahsAnim;


    private void Awake()
    {
        JogadorController = GetComponentInParent<JogadorController>();
        armaAtiva = GetComponentInParent<ArmaAtiva>();
        animator = GetComponent<Animator>();
        jogadorControls = new JogadorControls();
    }

    private void OnEnable() { jogadorControls.Enable(); }
     
    void Start()
    {
        jogadorControls.Combat.Attack.started += _ => Atacar();
    }

    private void Atacar()
    {
        animator.SetTrigger("Ataque");
        armaCollider.gameObject.SetActive(true);

        slahsAnim = Instantiate(slashAnimPrefab, slashAnimPontoSpawn.position, Quaternion.identity);
        slahsAnim.transform.parent = this.transform.parent;
    }

    public void FinalAnimAtacarEvent() { armaCollider.gameObject.SetActive(false); }

    public void SwingCimaFlipAnimEvent()
    {
        slahsAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (JogadorController.OlhandoEsq)
            slahsAnim.GetComponent<SpriteRenderer>().flipX = true;
    }
    public void SwingBaixoFlipAnimEvent()
    {
        slahsAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (JogadorController.OlhandoEsq)
            slahsAnim.GetComponent<SpriteRenderer>().flipX = true;
    }


    void Update()
    {
        MouseSeguirOffSet();
    }

    private void MouseSeguirOffSet()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 jogadorScreenPoint = Camera.main.WorldToScreenPoint(JogadorController.transform.position);

        float angulo = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < jogadorScreenPoint.x)
        { armaAtiva.transform.rotation = Quaternion.Euler(0, -180, angulo);
            armaCollider.transform.rotation = Quaternion.Euler(0, -180, 0); }
        else 
        { armaAtiva.transform.rotation = Quaternion.Euler(0, 0, angulo); 
            armaCollider.transform.rotation = Quaternion.Euler(0, 0, 0); }

    }
}
