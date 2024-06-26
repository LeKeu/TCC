using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Entrada : MonoBehaviour
{
    [SerializeField] private string nomeTransicao;

    private void Start()
    {
        if(nomeTransicao == SceneManagement.Instance.NomeCenaTransicao)
        {
            JogadorController.Instance.transform.position = this.transform.position;
        }
    }
}
