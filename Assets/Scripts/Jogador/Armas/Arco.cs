using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arco : MonoBehaviour, IArma
{
    [SerializeField] private Armas armasInfo;
    [SerializeField] GameObject flechaPrefab;
    [SerializeField] Transform flechaSpawnPonto;

    readonly int FIRE_HASH = Animator.StringToHash("Fire");

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Atacar()
    {
        //Debug.Log("arco");
        //ArmaAtiva.Instance.ToggleEstaAtacando(false);
        animator.SetTrigger(FIRE_HASH);
        GameObject novaFlecha = Instantiate(flechaPrefab, flechaSpawnPonto.position, ArmaAtiva.Instance.transform.rotation);
        novaFlecha.GetComponent<Projetil>().UpdateArmaInfo(armasInfo);
    }

    public Armas PegarArmaInfo()
    {
        return armasInfo;
    }
}
