using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destrutiveis : MonoBehaviour
{
    //[SerializeField] private GameObject vfxDestruir;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<OrigemDano>())
        {
            //Instantiate(vfxDestruir, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
