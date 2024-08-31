using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelhaNamia : NPCs, ITalkable
{
    [SerializeField] private List<DialogoTexto> dt;
    [SerializeField] private DialogoController dialogoController;

    [SerializeField] private GameObject ErvasVerdes;

    int indexAtual = 0;

    string nome = "Velha N�mia";
    [SerializeField] private Sprite perfil;
    public bool tutCompleto;

    public override void Interagir()
    {

        if (JogadorController.Instance.podeMover) // se o jogador pode se mover, no caso s� ocorre quando a conversa acabou
        {
            if (!tutCompleto)
                CompletarTutorial();

            if (tutCompleto) // s� vai sair do txt de tut qnd tiver completa a 'quest'
                indexAtual++;

            if (indexAtual == dt.Count && !tutCompleto) { indexAtual = 0; }
            if (indexAtual == dt.Count && tutCompleto) { indexAtual = 1; }; // index 0 sempre ser� o txt do tut, ent qnd ele estiver completo, n�o volta mais
        }
        if (!JogadorController.Instance.estaAndando)
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
        if(ErvasVerdes.transform.childCount == 0) 
            tutCompleto = true;
    }
}