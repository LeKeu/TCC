using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Saida : MonoBehaviour
{
    [SerializeField] private string proxCena;
    [SerializeField] private string cenaTransicaoNome;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<JogadorController>())
        { 
            SceneManagement.Instance.SetTransicaoNome(cenaTransicaoNome);
            SceneManager.LoadScene(proxCena);
        }
    }
}
