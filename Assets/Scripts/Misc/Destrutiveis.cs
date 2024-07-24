using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destrutiveis : MonoBehaviour
{
    //[SerializeField] private GameObject vfxDestruir;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<OrigemDano>() || collision.gameObject.GetComponent<Projetil>())
        {
            //Instantiate(vfxDestruir, transform.position, Quaternion.identity);
            GetComponent<PegarExtras>().DroparItens();
            Destroy(gameObject);
        }
    }
}
