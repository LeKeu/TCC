using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{
    [SerializeField] float movVel = 20f;
    [SerializeField] bool seguePlayer = false;
    //[SerializeField] private GameObject particulaAcertarObjPrefab;

    Armas armasInfo;
    Vector3 posInicial;

    private void Start()
    {
        posInicial = transform.position;
    }
    private void Update()
    {
        MoverProjetil();
        DetectarFireDistancia();
    }

    public void UpdateArmaInfo(Armas armasInfo)
    {
        this.armasInfo = armasInfo;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        InimigoVida inimigoVida = collision.GetComponent<InimigoVida>();
        Indestrutivel indestrutivel = collision.gameObject.GetComponent<Indestrutivel>();

        if (!collision.isTrigger && (inimigoVida || indestrutivel))
        {
            inimigoVida?.ReceberDano(armasInfo.armaDano);
            //Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    void DetectarFireDistancia()
    {
        if (Vector3.Distance(transform.position, posInicial) > armasInfo.armaRange)
        {
            Destroy(gameObject);
        }
    }

    private void MoverProjetil()
    {
        if(seguePlayer)
            transform.Translate(Vector3.right * Time.deltaTime * movVel);
        else
            transform.Translate(Vector3.right * Time.deltaTime * movVel); //TROCAR ISSO AQUI

    }
}
