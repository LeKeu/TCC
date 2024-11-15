using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelhaNamia : NPCs, ITalkable
{
    [SerializeField] private List<DialogoTexto> dt;
    [SerializeField] private DialogoController dialogoController;

    [SerializeField] private GameObject ErvasVerdes;

    int indexAtual = 0;

    [SerializeField] private Sprite perfil;
    public bool tutCompleto;

    [SerializeField] ArmaAtiva armaAtiva;
    [SerializeField] InventarioAtivo inventarioAtivo;

    public override void Interagir()
    {

        if (JogadorController.Instance.podeMover) // se o jogador pode se mover, no caso só ocorre quando a conversa acabou
        {
            if (!tutCompleto)
                CompletarTutorial();

            if (indexAtual == 0 && !tutCompleto)
            {
                armaAtiva.AtivarArma1(true);
                inventarioAtivo.AtivarArma1(true);
            }

            if (tutCompleto) // só vai sair do txt de tut qnd tiver completa a 'quest'
                indexAtual++;

            if (indexAtual == dt.Count && !tutCompleto) { indexAtual = 0; }
            if (indexAtual == dt.Count && tutCompleto) { indexAtual = 1; }; // index 0 sempre será o txt do tut, ent qnd ele estiver completo, não volta mais
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
        {
            tutCompleto = true;
            armaAtiva.AtivarArma1(false);       //
            inventarioAtivo.AtivarArma1(false); //
        }
    }
}