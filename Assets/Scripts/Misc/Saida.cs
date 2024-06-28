using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Saida : MonoBehaviour
{
    [SerializeField] private string proxCena;
    //[SerializeField] private string proyena;
    [SerializeField] private string cenaTransicaoNome;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<JogadorController>())
        { 
            SceneManager.LoadScene(proxCena);
            SceneManagement.Instance.SetTransicaoNome(cenaTransicaoNome);
        }
    }
}
