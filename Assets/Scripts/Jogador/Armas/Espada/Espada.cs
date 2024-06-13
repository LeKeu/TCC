using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espada : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimPontoSpawn;

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

        slahsAnim = Instantiate(slashAnimPrefab, slashAnimPontoSpawn.position, Quaternion.identity);
        slahsAnim.transform.parent = this.transform.parent;
    }

    public void SwingCimaFlipAnim()
    {
        slahsAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (JogadorController.OlhandoEsq)
            slahsAnim.GetComponent<SpriteRenderer>().flipX = true;
    }
    public void SwingBaixoFlipAnim()
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

        if(mousePos.x < jogadorScreenPoint.x) armaAtiva.transform.rotation = Quaternion.Euler(0, -180, angulo);
        else armaAtiva.transform.rotation = Quaternion.Euler(0, 0, angulo);

    }
}
