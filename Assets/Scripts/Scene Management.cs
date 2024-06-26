using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagement : Singleton<SceneManagement>
{
    public string NomeCenaTransicao {  get; private set; }

    public void SetTransicaoNome(string nomeCenaTransicao)
    {
        this.NomeCenaTransicao = nomeCenaTransicao;
    }
}
