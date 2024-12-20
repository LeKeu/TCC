using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoAtirador : MonoBehaviour, IInimigo
{
    [SerializeField] private GameObject projetilPrefab;

    //private Animator myAnimator;
    private SpriteRenderer spriteRenderer;
    InimigoVida inimigoVida;

    //readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private void Awake()
    {
        //myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        inimigoVida = GetComponent<InimigoVida>();
    }


    public void Atacar()
    {
        //myAnimator.SetTrigger(ATTACK_HASH);
        if(!inimigoVida.estaAtordoado)
            CriarProjetil();

        if (transform.position.x - JogadorController.Instance.transform.position.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }
    
    public void CriarProjetil()
    {
        Instantiate(projetilPrefab, transform.position, Quaternion.identity);
    }
}
