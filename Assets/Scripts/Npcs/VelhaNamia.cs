using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelhaNamia : NPCs, ITalkable
{
    [SerializeField] private List<DialogoTexto> dt;
    [SerializeField] private DialogoController dialogoController;

    [SerializeField] private GameObject ErvasVerdes;

    int indexAtual = 0;

    string nome = "Velha Nâmia";
    [SerializeField] private Sprite perfil;
    bool tutCompleto;

    //private void Update()
    //{
    //    if(!tutCompleto)
    //        CompletarTutorial();
    //}

    public override void Interagir()
    {

        if (JogadorController.Instance.podeMover) // se o jogador pode se mover, no caso só ocorre quando a conversa acabou
        {
            if (!tutCompleto)
                CompletarTutorial();

            if (tutCompleto)
                indexAtual++;
            Debug.Log("1indexatual = "+indexAtual);
            if (indexAtual == dt.Count && !tutCompleto) { indexAtual = 0; }
            if (indexAtual == dt.Count && tutCompleto) { indexAtual = 1; }
            Debug.Log("2indexatual = "+indexAtual);
        }
        Falar(dt[indexAtual]);
    }
    public void Falar(DialogoTexto dialogoTexto)
    {
        //dialogoTexto.nome = nome;
        dialogoTexto.perfilNPC = perfil;
        dialogoController.DisplayProximoParagrafo(dialogoTexto);
    }

    void CompletarTutorial()
    {
        if(ErvasVerdes.transform.childCount == 0) { Debug.Log("tut namia completo"); tutCompleto = true; }
    }
}