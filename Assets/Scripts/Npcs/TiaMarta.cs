using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiaMarta : NPCs, ITalkable
{
    [SerializeField] private List<DialogoTexto> dt; // LIST^2

    [SerializeField] private DialogoController dialogoController;

    int indexAtual = 0;

    string nome = "Dona Marta";
    [SerializeField] private Sprite perfil;

    public override void Interagir()
    {
        Falar(dt[indexAtual]);

        if (JogadorController.Instance.podeMover) // se o jogador pode se mover, no caso só ocorre quando a conversa acabou
        {
            indexAtual++;
            if (indexAtual == dt.Count) { indexAtual = 0; }
        }
    }
    public void Falar(DialogoTexto dialogoTexto)
    {
        dialogoTexto.nome = nome;
        dialogoTexto.perfilNPC = perfil;
        dialogoController.DisplayProximoParagrafo(dialogoTexto);
    }
}
