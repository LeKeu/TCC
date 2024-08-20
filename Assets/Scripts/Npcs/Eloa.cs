using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eloa : NPCs, ITalkable
{
    [SerializeField] private List<DialogoTexto> dt;

    [SerializeField] private DialogoController dialogoController;

    int indexAtual = 0;

    string nome = "Eloa";
    [SerializeField] private Sprite perfil;

    public override void Interagir()
    {
        Falar(dt[indexAtual]);

        if (JogadorController.Instance.podeMover) // se o jogador pode se mover, no caso s� ocorre quando a conversa acabou
        {
            indexAtual++;
            if (indexAtual == dt.Count) { indexAtual = 0; }
        }
    }
    public void Falar(DialogoTexto dialogoTexto)
    {
        //dialogoTexto.nome = nome;
        dialogoTexto.perfilNPC = perfil;
        dialogoController.DisplayProximoParagrafo(dialogoTexto);
    }
}
